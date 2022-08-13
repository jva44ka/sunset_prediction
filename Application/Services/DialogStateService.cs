using System;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using DataAccess.Dao.Interfaces;
using Domain.Entities.Enums;
using TelegramApi.Client.Entities;
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
            Message message)
        {
            var user = await _userDao.GetUserById(userId);
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

        public ReplyKeyboardMarkup? BuildKeyboard(DialogState dialogState)
        {
            switch (dialogState)
            {
                case DialogState.ProposedInputCity:
                    return null;
                case DialogState.ProposedFoundedCity:
                    return ReplyKeyboardMarkup.CreateFromButtonTexts("Да", "Нет");
                case DialogState.OfChoosingSubscribeType:
                    return ReplyKeyboardMarkup.CreateFromButtonTexts("Обычная", "Двойная");
                case DialogState.SubscribedToEverydayPushes:
                case DialogState.SubscribedToEverydayDoublePushes:
                    return null;
                case DialogState.SubscribedTriesToUnsubscribe:
                    return ReplyKeyboardMarkup.CreateFromButtonTexts("Да", "Нет");
                case DialogState.Unsubscribed:
                    return ReplyKeyboardMarkup.CreateFromButtonTexts("Подписка");
                case DialogState.UnsubscribedTriesSubscribe:
                    return ReplyKeyboardMarkup.CreateFromButtonTexts("Обычная", "Двойная");

                default:
                    throw new ArgumentOutOfRangeException(nameof(dialogState),
                                                          dialogState,
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
                    Message = $"Ваш город {city.Address}?",
                    NewState = userWithNewState.CurrentDialogState
                };
            }
            else
            {
                return new TransitionResult
                {
                    Message = "Город с таким названием не найден. Попробуйте написать точное название вашего города.",
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
                    Message = "Выбирите и напишите вариант рассылки:\n" +
                              "1. 'Обычная' - бот присылает одно сообщение за час до заката при высокой вероятности заката \n" +
                              "2. 'Двойная' - бот присылает одно сообщение с утра, второе сообщение за час до заката при высокой вероятности заката",
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
                    Message = "Возможно вы ввели неполное название города, попробуйте еще раз.",
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
                            Message = "Вы подписались на обычную рассылку. В случае успешного прогноза " +
                                      "заката вам будет отправлено уведомление за час.\n" +
                                      "Если вы хотите отписаться от рассылки напишите 'Отписка'.",
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
                            Message = "Вы подписались на двойную рассылку. В случае успешного прогноза заката вам будет " +
                                      "отправлено уведомление утром. Если вечером того же дня прогноз будет все еще успешен, " +
                                      "вам отправится второе уведомление за час до заката. \n" +
                                      "Если вы хотите отписаться от рассылки напишите 'Отписка'.",
                            NewState = userWithNewState.CurrentDialogState
                        };
                    }

                default:
                    return new TransitionResult
                    {
                        Message = "Введеный вариант подписки не распознан. Пожалуйста напишите один " +
                                  "из вариантов: 'Обычная' или 'Двойная'.",
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
                    Message = "Вы действительно хотите отписаться от рассылки?",
                    NewState = userWithNewState.CurrentDialogState
                };
            }
            else
            {
                return new TransitionResult
                {
                    Message = "Если вы хотите отписаться от рассылки напишите 'Отписка'.",
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
                    Message = "Вы отписаны от всех рассылок. \n" +
                              "Если вы хотите опять подписаться на рассылку прогноза - от рассылки напишите 'Подписка'",
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
                    Message = "Вы подписаны на рассылку. \n" +
                              "Если вы хотите отписаться от рассылки напишите 'Отписка'.",
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
                    Message = "Напишите один из вариантов подписки: 'Обычная' или 'Двойная'.",
                    NewState = userWithNewState.CurrentDialogState
                };
            }
            else
            {
                return new TransitionResult
                {
                    Message = "Вы отписаны от всех рассылок. \n" +
                              "Если вы хотите опять подписаться на рассылку прогноза - от рассылки напишите 'Подписка'",
                    NewState = user.CurrentDialogState
                };
            }
        }

        private async Task<TransitionResult> WithoutState(Message message)
        {
            var userWithNewState = new User
            {
                Id = message.From.Id,
                FirstName = message.From.FirstName,
                LastName = message.From.LastName,
                UserName = message.From.Username,

                CurrentDialogState = DialogState.ProposedInputCity,
                StateChangeDate = DateTime.UtcNow
            };
            await _userDao.Create(userWithNewState);

            return new TransitionResult
            {
                Message = "Пожалуйста введите название своего города (желательно точное название).",
                NewState = userWithNewState.CurrentDialogState
            };
        }

        public class TransitionResult
        {
            public string Message { get; set; }
            public DialogState NewState { get; set; }
        }
    }
}