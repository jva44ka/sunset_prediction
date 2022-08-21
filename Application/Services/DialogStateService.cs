using System;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using DataAccess.Dao.Interfaces;
using Domain.Entities.Enums;
using TelegramApi.Client.Dtos;
using User = Domain.Entities.User;

namespace Application.Services
{
    public class DialogStateService : IDialogStateService
    {
        private readonly ICitiesParserService _citiesParserService;
        private readonly IUserDao _userDao;
        private readonly ICityDao _cityDao;

        public DialogStateService(
            ICitiesParserService citiesParserService,
            IUserDao userDao,
            ICityDao cityDao)
        {
            _citiesParserService = citiesParserService;
            _userDao = userDao;
            _cityDao = cityDao;
        }

        public async Task<TransitionResult> TransitionState(
            int userId, 
            MessageDto message)
        {
            var user = await _userDao.GetByExternalId(userId);
            if (user == null)
            {
                return await WithoutState(message);
            }

            switch (user.CurrentDialogState)
            {
                case DialogState.ProposedInputCity:
                    return await ProposedInputCity(user, message.Text);
                case DialogState.ProposedFoundedCity:
                    return await ProposedFoundedCity(user, message.Text);
                case DialogState.OfChoosingSubscribeType:
                    return await OfChoosingSubscribeType(user, message.Text);
                case DialogState.SubscribedToEverydayPushes:
                case DialogState.SubscribedToEverydayDoublePushes:
                    return await SubscribedToPushes(user, message.Text);
                case DialogState.SubscribedTriesToUnsubscribe:
                    return await SubscribedTriesToUnsubscribe(user, message.Text);
                case DialogState.Unsubscribed:
                    return await Unsubscribed(user, message.Text);
                case DialogState.UnsubscribedTriesSubscribe:
                    return await OfChoosingSubscribeType(user, message.Text);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(user.CurrentDialogState), 
                        user.CurrentDialogState, 
                        null);

            }
        }


        private async Task<TransitionResult> ProposedInputCity(
            User user, 
            string messageText)
        {
            var isCityExistsInDb = true;
            var city = await _cityDao.GetCityByLowerCaseName(messageText.Trim().ToLower());
            if (city == null)
            {
                isCityExistsInDb = false;
                city = await _citiesParserService.FindCity(messageText);
            }

            if (city != null)
            {
                var userWithNewState = new User
                {
                    PreviousDialogState = user.CurrentDialogState,
                    CurrentDialogState = DialogState.ProposedFoundedCity,
                    Id = user.Id,
                    CityId = city.Id,
                    StateChangeDate = DateTime.UtcNow
                };

                if (isCityExistsInDb == false)
                {
                    await _cityDao.Create(city);
                }
                await _userDao.Update(userWithNewState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.ProposedCityName,
                    NewState = userWithNewState.CurrentDialogState,
                    CityAddress = city.Address
                };
            }
            else
            {
                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.CityNameNotFound,
                    NewState = user.CurrentDialogState
                };
            }
        }

        private async Task<TransitionResult> ProposedFoundedCity(
            User currentUserState, 
            string messageText)
        {
            if (messageText.Trim().ToLower() == "да")
            {
                var userWithNewState = new User
                {
                    Id = currentUserState.Id,
                    PreviousDialogState = currentUserState.CurrentDialogState,
                    CurrentDialogState = DialogState.OfChoosingSubscribeType,
                    CityId = currentUserState.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(userWithNewState);

                var cityInDb = await _cityDao.GetCityById(currentUserState.CityId.Value);
                if (cityInDb == null)
                {
                    var city = await _citiesParserService.FindCity(currentUserState.CityId.Value);
                    await _cityDao.Create(city);
                }

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.ProposedSubscribeTypes,
                    NewState = userWithNewState.CurrentDialogState
                };
            }
            else
            {
                var userWithNewState = new User
                {
                    Id = currentUserState.Id,
                    PreviousDialogState = currentUserState.CurrentDialogState,
                    CurrentDialogState = DialogState.ProposedInputCity,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(userWithNewState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.ProposedCityNameWrong,
                    NewState = userWithNewState.CurrentDialogState
                };
            }
        }

        private async Task<TransitionResult> OfChoosingSubscribeType(
            User user, 
            string messageText)
        {
            switch (messageText.Trim().ToLower())
            {
                case "обычная":
                    {
                        var userWithNewState = new User
                        {
                            Id = user.Id,
                            PreviousDialogState = user.CurrentDialogState,
                            CurrentDialogState = DialogState.SubscribedToEverydayPushes,
                            CityId = user.CityId,
                            StateChangeDate = DateTime.UtcNow
                        };
                        await _userDao.Update(userWithNewState);

                        return new TransitionResult
                        {
                            AnswerMessageType = AnswerMessageType.SubscribedToEverydayPushes,
                            NewState = userWithNewState.CurrentDialogState
                        };
                    }

                case "двойная":
                    {
                        var userWithNewState = new User
                        {
                            Id = user.Id,
                            PreviousDialogState = user.CurrentDialogState,
                            CurrentDialogState = DialogState.SubscribedToEverydayDoublePushes,
                            CityId = user.CityId,
                            StateChangeDate = DateTime.UtcNow
                        };
                        await _userDao.Update(userWithNewState);

                        return new TransitionResult
                        {
                            AnswerMessageType = AnswerMessageType.SubscribedToEverydayDoublePushes,
                            NewState = userWithNewState.CurrentDialogState
                        };
                    }

                default:
                    return new TransitionResult
                    {
                        AnswerMessageType = AnswerMessageType.InputSubscribeNameWrong,
                        NewState = user.CurrentDialogState
                    };
            }
        }

        private async Task<TransitionResult> SubscribedToPushes(
            User user,
            string messageText)
        {
            if (messageText.Trim().ToLower() == "отписка")
            {
                var userWithNewState = new User
                {
                    Id = user.Id,
                    PreviousDialogState = user.CurrentDialogState,
                    CurrentDialogState = DialogState.SubscribedTriesToUnsubscribe,
                    CityId = user.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(userWithNewState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.UnsubscribeWarning,
                    NewState = userWithNewState.CurrentDialogState
                };
            }
            else
            {
                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.StaysSubscribed,
                    NewState = user.CurrentDialogState
                };
            }
        }

        private async Task<TransitionResult> SubscribedTriesToUnsubscribe(
            User user,
            string messageText)
        {
            if (messageText.Trim().ToLower() == "да")
            {
                var userWithNewState = new User
                {
                    Id = user.Id,
                    PreviousDialogState = user.CurrentDialogState,
                    CurrentDialogState = DialogState.Unsubscribed,
                    CityId = user.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(userWithNewState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.Unsubscribed,
                    NewState = userWithNewState.CurrentDialogState
                };
            }
            else
            {
                var userWithNewState = new User
                {
                    Id = user.Id,
                    PreviousDialogState = user.CurrentDialogState,
                    CurrentDialogState = user.PreviousDialogState.Value,
                    CityId = user.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(userWithNewState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.StaysSubscribed,
                    NewState = userWithNewState.CurrentDialogState
                };
            }
        }

        private async Task<TransitionResult> Unsubscribed(
            User user,
            string messageText)
        {
            if (messageText.Trim().ToLower() == "подписка")
            {
                var userWithNewState = new User
                {
                    Id = user.Id,
                    PreviousDialogState = user.CurrentDialogState,
                    CurrentDialogState = DialogState.UnsubscribedTriesSubscribe,
                    CityId = user.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(userWithNewState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.ProposedSubscribeTypes,
                    NewState = userWithNewState.CurrentDialogState
                };
            }
            else
            {
                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.Unsubscribed,
                    NewState = user.CurrentDialogState
                };
            }
        }

        private async Task<TransitionResult> WithoutState(MessageDto message)
        {
            var userWithNewState = new User
            {
                ExternalId = message.From.Id,
                FirstName = message.From.FirstName,
                LastName = message.From.LastName,
                UserName = message.From.Username,

                CurrentDialogState = DialogState.ProposedInputCity,
                StateChangeDate = DateTime.UtcNow
            };
            await _userDao.Create(userWithNewState);

            return new TransitionResult
            {
                AnswerMessageType = AnswerMessageType.InputCity,
                NewState = userWithNewState.CurrentDialogState
            };
        }

        public class TransitionResult
        {
            public AnswerMessageType AnswerMessageType { get; set; }
            public DialogState NewState { get; set; }
            public string? CityAddress { get; set; }
        }
    }
}