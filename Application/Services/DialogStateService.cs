using System;
using System.Threading.Tasks;
using Application.Services.Dto;
using Application.Services.EntityServices.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Entities.Enums;
using TelegramApi.Client.Dtos;

namespace Application.Services
{
    public class DialogStateService : IDialogStateService
    {
        private readonly IUserService _userService;
        private readonly ICityService _cityService;

        public DialogStateService(
            IUserService userService,
            ICityService cityService)
        {
            _userService = userService;
            _cityService = cityService;
        }

        public async Task<TransitionResult> TransitionState(
            int userId,
            MessageDto message)
        {
            var user = await _userService.GetByExternalId(userId);
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
            var city = await _cityService.GetCityByLowerCaseName(messageText);
            if (city != null)
            {
                var newState = DialogState.ProposedFoundedCity;
                await _userService.UpdateState(user.ExternalId, newState);
                await _userService.UpdateCity(user.ExternalId, city.Id);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.ProposedCityName,
                    NewState = newState,
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
            User user,
            string messageText)
        {
            if (messageText.Trim().ToLower() == "да")
            {
                var newState = DialogState.OfChoosingSubscribeType;
                await _userService.UpdateState(user.ExternalId, newState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.ProposedSubscribeTypes,
                    NewState = newState
                };
            }
            else
            {
                var newState = DialogState.ProposedInputCity;
                await _userService.UpdateState(user.ExternalId, newState);
                await _userService.UpdateCity(user.ExternalId, null);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.ProposedCityNameWrong,
                    NewState = newState
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
                        var newState = DialogState.SubscribedToEverydayPushes;
                        await _userService.UpdateState(user.ExternalId, newState);

                        return new TransitionResult
                        {
                            AnswerMessageType = AnswerMessageType.SubscribedToEverydayPushes,
                            NewState = newState
                        };
                    }

                case "двойная":
                    {
                        var newState = DialogState.SubscribedToEverydayDoublePushes;
                        await _userService.UpdateState(user.ExternalId, newState);

                        return new TransitionResult
                        {
                            AnswerMessageType = AnswerMessageType.SubscribedToEverydayDoublePushes,
                            NewState = newState
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
                var newState = DialogState.SubscribedTriesToUnsubscribe;
                await _userService.UpdateState(user.ExternalId, newState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.UnsubscribeWarning,
                    NewState = newState
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
                var newState = DialogState.Unsubscribed;
                await _userService.UpdateState(user.ExternalId, newState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.Unsubscribed,
                    NewState = newState
                };
            }
            else
            {
                var newState = user.PreviousDialogState
                               ?? throw new Exception(
                                   $"User has not previous state with external id: {user.ExternalId}");
                await _userService.UpdateState(user.ExternalId, newState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.StaysSubscribed,
                    NewState = newState
                };
            }
        }

        private async Task<TransitionResult> Unsubscribed(
            User user,
            string messageText)
        {
            if (messageText.Trim().ToLower() == "подписка")
            {
                var newState = DialogState.UnsubscribedTriesSubscribe;
                await _userService.UpdateState(user.ExternalId, newState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.ProposedSubscribeTypes,
                    NewState = newState
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
            var user = new User
            {
                ExternalId = message.From.Id,
                FirstName = message.From.FirstName,
                LastName = message.From.LastName,
                UserName = message.From.Username,

                CurrentDialogState = DialogState.ProposedInputCity,
                StateChangeDate = DateTime.UtcNow
            };
            await _userService.Create(user);

            return new TransitionResult
            {
                AnswerMessageType = AnswerMessageType.InputCity,
                NewState = user.CurrentDialogState
            };
        }
    }
}