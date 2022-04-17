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

        public Task<string> TransitionState(User? currentUserState, Message message)
        {
            if (currentUserState == null)
            {
                return WithoutState(message);
            }

            switch (currentUserState.DialogState)
            {
                case DialogState.ProposedInputCity:
                    return ProposedInputCity(currentUserState, message);
                case DialogState.ProposedFoundedCity:
                    return ProposedFoundedCity(currentUserState, message);
                case DialogState.OfChoosingSubscribeType:
                    return OfChoosingSubscribeType(currentUserState, message);
                case DialogState.SubscribedToEverydayPushes:
                case DialogState.SubscribedToEverydayDoublePushes:
                    return SubscribedToPushes(currentUserState, message);
                case DialogState.SubscribedTriesToUnsubscribe:
                    return SubscribedTriesToUnsubscribe(currentUserState, message);
                case DialogState.Unsubscribed:
                    return Unsubscribed(currentUserState, message);
                case DialogState.UnsubscribedTriesSubscribe:
                    return OfChoosingSubscribeType(currentUserState, message);
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentUserState.DialogState), currentUserState.DialogState, null);

            }
        }


        private async Task<string> ProposedInputCity(User currentUserState,
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
                var newUserState = new User
                {
                    PreviousDialogState = currentUserState.DialogState,
                    DialogState = DialogState.ProposedFoundedCity,
                    Id = currentUserState.Id,
                    CityId = city.Id,
                    StateChangeDate = DateTime.UtcNow
                };

                if (cityNotExistInDB)
                {
                    await _cityDao.Create(city)
                                  .ConfigureAwait(false);
                }
                await _userDao.Update(newUserState)
                              .ConfigureAwait(false);

                return $"Ваш город {city.Address}?";
            }

            return "Город с таким названием не найден. Попробуйте написать точное название вашего города.";
        }

        private async Task<string> ProposedFoundedCity(User currentUserState,
                                                       Message message)
        {
            if (message.Text.Trim().ToLower() == "да")
            {
                var newUserState = new User
                {
                    Id = currentUserState.Id,
                    PreviousDialogState = currentUserState.DialogState,
                    DialogState = DialogState.OfChoosingSubscribeType,
                    CityId = currentUserState.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(newUserState).ConfigureAwait(false);

                var city = await _citiesParserService.FindCity(currentUserState.CityId.Value)
                                                     .ConfigureAwait(false);
                var cityInDb = await _cityDao.GetCityById(city.Id).ConfigureAwait(false);
                if (cityInDb == null)
                {
                    await _cityDao.Create(city).ConfigureAwait(false);
                }

                return "Выбирите и напишите вариант рассылки:\n" +
                       "1. 'Обычная' - бот присылает одно сообщение за час до заката при высокой вероятности заката \n" +
                       "2. 'Двойная' - бот присылает одно сообщение с утра, второе сообщение за час до заката при высокой вероятности заката";
            }
            else
            {
                var newUserState = new User
                {
                    Id = currentUserState.Id,
                    PreviousDialogState = currentUserState.DialogState,
                    DialogState = DialogState.ProposedInputCity,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(newUserState).ConfigureAwait(false);

                return "Возможно вы ввели неполное название города, попробуйте еще раз.";
            }
        }

        private async Task<string> OfChoosingSubscribeType(User currentUserState,
                                                           Message message)
        {
            switch (message.Text.Trim().ToLower())
            {
                case "обычная":
                    {
                        var newUserState = new User
                        {
                            Id = currentUserState.Id,
                            PreviousDialogState = currentUserState.DialogState,
                            DialogState = DialogState.SubscribedToEverydayPushes,
                            CityId = currentUserState.CityId,
                            StateChangeDate = DateTime.UtcNow
                        };
                        await _userDao.Update(newUserState).ConfigureAwait(false);

                        return
                            "Вы подписались на обычную рассылку. В случае успешного прогноза заката вам будет отправлено уведомление за час.\n" +
                            "Если вы хотите отписаться от рассылки напишите 'Отписка'.";
                    }

                case "двойная":
                    {
                        var newUserState = new User
                        {
                            Id = currentUserState.Id,
                            PreviousDialogState = currentUserState.DialogState,
                            DialogState = DialogState.SubscribedToEverydayDoublePushes,
                            CityId = currentUserState.CityId,
                            StateChangeDate = DateTime.UtcNow
                        };
                        await _userDao.Update(newUserState).ConfigureAwait(false);

                        return
                            "Вы подписались на двойную рассылку. В случае успешного прогноза заката вам будет отправлено уведомление утром. " +
                            "Если вечером того же дня прогноз будет все еще успешен, вам отправится второе уведомление за час до заката. \n" +
                            "Если вы хотите отписаться от рассылки напишите 'Отписка'.";
                    }

                default:
                    return "Введеный вариант подписки не распознан. Пожалуйста напишите один из вариантов: 'Обычная' или 'Двойная'.";
            }
        }

        private async Task<string> SubscribedToPushes(User currentUserState,
                                                      Message message)
        {
            if (message.Text.Trim().ToLower() == "отписка")
            {
                var newUserState = new User
                {
                    Id = currentUserState.Id,
                    PreviousDialogState = currentUserState.DialogState,
                    DialogState = DialogState.SubscribedTriesToUnsubscribe,
                    CityId = currentUserState.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(newUserState).ConfigureAwait(false);

                return
                    "Вы действительно хотите отписаться от рассылки?";
            }

            return "Если вы хотите отписаться от рассылки напишите 'Отписка'.";
        }

        private async Task<string> SubscribedTriesToUnsubscribe(User currentUserState,
                                                                Message message)
        {
            if (message.Text.Trim().ToLower() == "да")
            {
                var newUserState = new User
                {
                    Id = currentUserState.Id,
                    PreviousDialogState = currentUserState.DialogState,
                    DialogState = DialogState.Unsubscribed,
                    CityId = currentUserState.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(newUserState).ConfigureAwait(false);

                return
                    "Вы отписаны от всех рассылок. \n" +
                    "Если вы хотите опять подписаться на рассылку прогноза - от рассылки напишите 'Подписка'";
            }
            else
            {
                var newUserState = new User
                {
                    Id = currentUserState.Id,
                    PreviousDialogState = currentUserState.DialogState,
                    DialogState = currentUserState.PreviousDialogState.Value,
                    CityId = currentUserState.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(newUserState).ConfigureAwait(false);

                return "Вы подписаны на рассылку. \n" +
                       "Если вы хотите отписаться от рассылки напишите 'Отписка'.";
            }
        }

        private async Task<string> Unsubscribed(User currentUserState,
                                                Message message)
        {
            if (message.Text.Trim().ToLower() == "подписка")
            {
                var newUserState = new User
                {
                    Id = currentUserState.Id,
                    PreviousDialogState = currentUserState.DialogState,
                    DialogState = DialogState.UnsubscribedTriesSubscribe,
                    CityId = currentUserState.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _userDao.Update(newUserState).ConfigureAwait(false);

                return "Напишите один из вариантов подписки: 'Обычная' или 'Двойная'.";
            }

            return "Вы отписаны от всех рассылок. \n" +
                   "Если вы хотите опять подписаться на рассылку прогноза - от рассылки напишите 'Подписка'";
        }

        private async Task<string> WithoutState(Message message)
        {
            var newUserState = new User
            {
                Id = message.From.Id,
                FirstName = message.From.FirstName,
                LastName = message.From.LastName,
                UserName = message.From.Username,

                DialogState = DialogState.ProposedInputCity,
                StateChangeDate = DateTime.UtcNow
            };
            await _userDao.Create(newUserState).ConfigureAwait(false);

            return "Пожалуйста введите название своего города (желательно точное название).";
        }
    }
}