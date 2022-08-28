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
        private readonly IChatService _chatService;
        private readonly ICityService _cityService;

        public DialogStateService(
            IUserService userService,
            IChatService chatService,
            ICityService cityService)
        {
            _userService = userService;
            _chatService = chatService;
            _cityService = cityService;
        }

        public async Task<TransitionResult> TransitionState(
            MessageDto messageDto)
        {
            var chatExternalId = messageDto.Chat.Id; 
            var userDto = messageDto.From;
            var messageText = messageDto.Text;

            var chat = await _chatService.GetByExternalId(chatExternalId);
            var user = await _userService.GetByExternalId(userDto.Id);
            if (chat == null)
            {
                return await WithoutState(chatExternalId, userDto);
            }

            switch (chat.CurrentState)
            {
                case ChatState.ProposedInputCity:
                    return await ProposedInputCity(chat, user, messageText);
                case ChatState.ProposedFoundedCity:
                    return await ProposedFoundedCity(chat, user, messageText);
                case ChatState.OfChoosingSubscribeType:
                    return await OfChoosingSubscribeType(chat, messageText);
                case ChatState.SubscribedToEverydayPushes:
                case ChatState.SubscribedToEverydayDoublePushes:
                    return await SubscribedToPushes(chat, messageText);
                case ChatState.SubscribedTriesToUnsubscribe:
                    return await SubscribedTriesToUnsubscribe(chat, messageText);
                case ChatState.Unsubscribed:
                    return await Unsubscribed(chat, messageText);
                case ChatState.UnsubscribedTriesSubscribe:
                    return await OfChoosingSubscribeType(chat, messageText);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(chat.CurrentState),
                        chat.CurrentState,
                        null);

            }
        }


        private async Task<TransitionResult> ProposedInputCity(
            Chat chat,
            User user,
            string messageText)
        {
            var city = await _cityService.GetCityByLowerCaseName(messageText);
            if (city != null)
            {
                var newState = ChatState.ProposedFoundedCity;
                await _chatService.UpdateState(chat.ExternalId, newState);
                await _userService.UpdateCity(user.ExternalId, city.Id);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.ProposedCityName,
                    NewState = newState,
                    AnswerMessageArgs = new[] { city.Address }
                };
            }
            else
            {
                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.CityNameNotFound,
                    NewState = chat.CurrentState
                };
            }
        }

        private async Task<TransitionResult> ProposedFoundedCity(
            Chat chat,
            User user,
            string messageText)
        {
            if (messageText.Trim().ToLower() == "да")
            {
                var newState = ChatState.OfChoosingSubscribeType;
                await _chatService.UpdateState(chat.ExternalId, newState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.ProposedSubscribeTypes,
                    NewState = newState
                };
            }
            else
            {
                var newState = ChatState.ProposedInputCity;
                await _chatService.UpdateState(chat.ExternalId, newState);
                await _userService.UpdateCity(user.ExternalId, null);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.ProposedCityNameWrong,
                    NewState = newState
                };
            }
        }

        private async Task<TransitionResult> OfChoosingSubscribeType(
            Chat chat,
            string messageText)
        {
            switch (messageText.Trim().ToLower())
            {
                case "обычная":
                    {
                        var newState = ChatState.SubscribedToEverydayPushes;
                        await _chatService.UpdateState(chat.ExternalId, newState);

                        return new TransitionResult
                        {
                            AnswerMessageType = AnswerMessageType.SubscribedToEverydayPushes,
                            NewState = newState
                        };
                    }

                case "двойная":
                    {
                        var newState = ChatState.SubscribedToEverydayDoublePushes;
                        await _chatService.UpdateState(chat.ExternalId, newState);

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
                        NewState = chat.CurrentState
                    };
            }
        }

        private async Task<TransitionResult> SubscribedToPushes(
            Chat chat,
            string messageText)
        {
            if (messageText.Trim().ToLower() == "отписка")
            {
                var newState = ChatState.SubscribedTriesToUnsubscribe;
                await _chatService.UpdateState(chat.ExternalId, newState);

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
                    NewState = chat.CurrentState
                };
            }
        }

        private async Task<TransitionResult> SubscribedTriesToUnsubscribe(
            Chat chat,
            string messageText)
        {
            if (messageText.Trim().ToLower() == "да")
            {
                var newState = ChatState.Unsubscribed;
                await _chatService.UpdateState(chat.ExternalId, newState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.Unsubscribed,
                    NewState = newState
                };
            }
            else
            {
                var newState = chat.PreviousState
                               ?? throw new Exception(
                                   $"User has not previous state with external id: {chat.ExternalId}");
                await _chatService.UpdateState(chat.ExternalId, newState);

                return new TransitionResult
                {
                    AnswerMessageType = AnswerMessageType.StaysSubscribed,
                    NewState = newState
                };
            }
        }

        private async Task<TransitionResult> Unsubscribed(
            Chat chat,
            string messageText)
        {
            if (messageText.Trim().ToLower() == "подписка")
            {
                var newState = ChatState.UnsubscribedTriesSubscribe;
                await _chatService.UpdateState(chat.ExternalId, newState);

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
                    NewState = chat.CurrentState
                };
            }
        }

        private async Task<TransitionResult> WithoutState(
            long chatExternalId,
            UserDto userDto)
        {
            var chat = new Chat
            {
                ExternalId = chatExternalId,
                CurrentState = ChatState.ProposedInputCity,
                StateChangedAt = DateTime.UtcNow
            };
            await _chatService.Create(chat);

            var existingChat = await _chatService.GetByExternalId(chatExternalId);
            var user = new User
            {
                ExternalId = userDto.Id,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.Username,
                ChatId = existingChat.Id
            };
            await _userService.Create(user);

            return new TransitionResult
            {
                AnswerMessageType = AnswerMessageType.InputCity,
                NewState = chat.CurrentState
            };
        }
    }
}