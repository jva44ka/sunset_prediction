using System;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using DataAccess.DAO.Interfaces;
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

        public DialogStateService(ICitiesParserService citiesParserService,
                                  IUserDao userDao,
                                  ICityDao cityDao)
        {
            _citiesParserService = citiesParserService;
            _userDao = userDao;
            _cityDao = cityDao;
        }

        public Task<TransitionResult> TransitionState(User? user, Message message)
        {
            if (user == null)
            {
                return WithoutState(message);
            }

            switch (user.DialogState)
            {
                case DialogState.ProposedInputCity:
                    return ProposedInputCity(user, message);
                case DialogState.ProposedFoundedCity:
                    return ProposedFoundedCity(user, message);
                case DialogState.OfChoosingSubscribeType:
                    return OfChoosingSubscribeType(user, message);
                case DialogState.SubscribedToEverydayPushes:
                case DialogState.SubscribedToEverydayDoublePushes:
                    return SubscribedToPushes(user, message);
                case DialogState.SubscribedTriesToUnsubscribe:
                    return SubscribedTriesToUnsubscribe(user, message);
                case DialogState.Unsubscribed:
                    return Unsubscribed(user, message);
                case DialogState.UnsubscribedTriesSubscribe:
                    return OfChoosingSubscribeType(user, message);
                default:
                    throw new ArgumentOutOfRangeException(nameof(user.DialogState), user.DialogState, null);

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
            Message message)
        {
            var cityNotExistInDB = false;
            var city = await _cityDao.GetCityByLowerCaseName(message.Text.Trim().ToLower())
                                     .ConfigureAwait(false);
            if (city == null)
            {
                cityNotExistInDB = true;
                city = await _citiesParserService.FindCity(message.Text)
                                                 .ConfigureAwait(false);
            }

            if (city != null)
            {
                var userWithNewState = new User
                {
                    PreviousDialogState = user.DialogState,
                    DialogState = DialogState.ProposedFoundedCity,
                    Id = user.Id,
                    CityId = city.Id,
                    StateChangeDate = DateTime.UtcNow
                };

                if (cityNotExistInDB)
                {
                    await _cityDao.Create(city)
                                  .ConfigureAwait(false);
                }
                await _userDao.Update(userWithNewState)
                              .ConfigureAwait(false);

                return new TransitionResult
                {
                    Message = $"Ваш город {city.Address}?",
                    NewState = userWithNewState.DialogState
                };
            }
            else
            {
                return new TransitionResult
                {
                    Message = "Город с таким названием не найден. Попробуйте написать точное название вашего города.",
                    NewState = user.DialogState
                };
            }
        }

        private async Task<TransitionResult> ProposedFoundedCity(
            User currentUserState, 
            Message message)
        {
            if (message.Text.Trim().ToLower() == "да")
            {
                var userWithNewState = new User
                {
                    Id = currentUserState.Id,
                    PreviousDialogState = currentUserState.DialogState,
                    DialogState = DialogState.OfChoosingSubscribeType,
                    CityId = currentUserState.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(userWithNewState).ConfigureAwait(false);

                var city = await _citiesParserService.FindCity(currentUserState.CityId.Value)
                                                     .ConfigureAwait(false);
                var cityInDb = await _cityDao.GetCityById(city.Id).ConfigureAwait(false);
                if (cityInDb == null)
                {
                    await _cityDao.Create(city).ConfigureAwait(false);
                }

                return new TransitionResult
                {
                    Message = "Выбирите и напишите вариант рассылки:\n" +
                              "1. 'Обычная' - бот присылает одно сообщение за час до заката при высокой вероятности заката \n" +
                              "2. 'Двойная' - бот присылает одно сообщение с утра, второе сообщение за час до заката при высокой вероятности заката",
                    NewState = userWithNewState.DialogState
                };
            }
            else
            {
                var userWithNewState = new User
                {
                    Id = currentUserState.Id,
                    PreviousDialogState = currentUserState.DialogState,
                    DialogState = DialogState.ProposedInputCity,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(userWithNewState)
                              .ConfigureAwait(false);

                return new TransitionResult
                {
                    Message = "Возможно вы ввели неполное название города, попробуйте еще раз.",
                    NewState = userWithNewState.DialogState
                };
            }
        }

        private async Task<TransitionResult> OfChoosingSubscribeType(
            User user, 
            Message message)
        {
            switch (message.Text.Trim().ToLower())
            {
                case "обычная":
                    {
                        var userWithNewState = new User
                        {
                            Id = user.Id,
                            PreviousDialogState = user.DialogState,
                            DialogState = DialogState.SubscribedToEverydayPushes,
                            CityId = user.CityId,
                            StateChangeDate = DateTime.UtcNow
                        };
                        await _userDao.Update(userWithNewState)
                                      .ConfigureAwait(false);

                        return new TransitionResult
                        {
                            Message = "Вы подписались на обычную рассылку. В случае успешного прогноза " +
                                      "заката вам будет отправлено уведомление за час.\n" +
                                      "Если вы хотите отписаться от рассылки напишите 'Отписка'.",
                            NewState = userWithNewState.DialogState
                        };
                    }

                case "двойная":
                    {
                        var userWithNewState = new User
                        {
                            Id = user.Id,
                            PreviousDialogState = user.DialogState,
                            DialogState = DialogState.SubscribedToEverydayDoublePushes,
                            CityId = user.CityId,
                            StateChangeDate = DateTime.UtcNow
                        };
                        await _userDao.Update(userWithNewState)
                                      .ConfigureAwait(false);

                        return new TransitionResult
                        {
                            Message = "Вы подписались на двойную рассылку. В случае успешного прогноза заката вам будет " +
                                      "отправлено уведомление утром. Если вечером того же дня прогноз будет все еще успешен, " +
                                      "вам отправится второе уведомление за час до заката. \n" +
                                      "Если вы хотите отписаться от рассылки напишите 'Отписка'.",
                            NewState = userWithNewState.DialogState
                        };
                    }

                default:
                    return new TransitionResult
                    {
                        Message = "Введеный вариант подписки не распознан. Пожалуйста напишите один " +
                                  "из вариантов: 'Обычная' или 'Двойная'.",
                        NewState = user.DialogState
                    };
            }
        }

        private async Task<TransitionResult> SubscribedToPushes(
            User user,
            Message message)
        {
            if (message.Text.Trim().ToLower() == "отписка")
            {
                var userWithNewState = new User
                {
                    Id = user.Id,
                    PreviousDialogState = user.DialogState,
                    DialogState = DialogState.SubscribedTriesToUnsubscribe,
                    CityId = user.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(userWithNewState)
                              .ConfigureAwait(false);

                return new TransitionResult
                {
                    Message = "Вы действительно хотите отписаться от рассылки?",
                    NewState = userWithNewState.DialogState
                };
            }
            else
            {
                return new TransitionResult
                {
                    Message = "Если вы хотите отписаться от рассылки напишите 'Отписка'.",
                    NewState = user.DialogState
                };
            }
        }

        private async Task<TransitionResult> SubscribedTriesToUnsubscribe(
            User user,
            Message message)
        {
            if (message.Text.Trim().ToLower() == "да")
            {
                var userWithNewState = new User
                {
                    Id = user.Id,
                    PreviousDialogState = user.DialogState,
                    DialogState = DialogState.Unsubscribed,
                    CityId = user.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(userWithNewState)
                              .ConfigureAwait(false);

                return new TransitionResult
                {
                    Message = "Вы отписаны от всех рассылок. \n" +
                              "Если вы хотите опять подписаться на рассылку прогноза - от рассылки напишите 'Подписка'",
                    NewState = userWithNewState.DialogState
                };
            }
            else
            {
                var userWithNewState = new User
                {
                    Id = user.Id,
                    PreviousDialogState = user.DialogState,
                    DialogState = user.PreviousDialogState.Value,
                    CityId = user.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(userWithNewState)
                              .ConfigureAwait(false);

                return new TransitionResult
                {
                    Message = "Вы подписаны на рассылку. \n" +
                              "Если вы хотите отписаться от рассылки напишите 'Отписка'.",
                    NewState = userWithNewState.DialogState
                };
            }
        }

        private async Task<TransitionResult> Unsubscribed(User user,
                                                Message message)
        {
            if (message.Text.Trim().ToLower() == "подписка")
            {
                var userWithNewState = new User
                {
                    Id = user.Id,
                    PreviousDialogState = user.DialogState,
                    DialogState = DialogState.UnsubscribedTriesSubscribe,
                    CityId = user.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(userWithNewState)
                              .ConfigureAwait(false);

                return new TransitionResult
                {
                    Message = "Напишите один из вариантов подписки: 'Обычная' или 'Двойная'.",
                    NewState = userWithNewState.DialogState
                };
            }
            else
            {
                return new TransitionResult
                {
                    Message = "Вы отписаны от всех рассылок. \n" +
                              "Если вы хотите опять подписаться на рассылку прогноза - от рассылки напишите 'Подписка'",
                    NewState = user.DialogState
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

                DialogState = DialogState.ProposedInputCity,
                StateChangeDate = DateTime.UtcNow
            };
            await _userDao.Create(userWithNewState)
                          .ConfigureAwait(false);

            return new TransitionResult
            {
                Message = "Пожалуйста введите название своего города (желательно точное название).",
                NewState = userWithNewState.DialogState
            };
        }

        public class TransitionResult
        {
            public string Message { get; set; }
            public DialogState NewState { get; set; }
        }
    }
}