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
        private readonly IDialogStateDao _dialogStateDao;
        private readonly ICityDao _cityDao;

        public DialogStateService(ICitiesParserService citiesParserService,
                                  IDialogStateDao dialogStateDao,
                                  ICityDao cityDao)
        {
            _citiesParserService = citiesParserService;
            _dialogStateDao = dialogStateDao;
            _cityDao = cityDao;
        }

        //TODO: Почему User currentState? разделить юзера и состояние диалога и выделить машину состояний в
        //TODO: отдельную сущность(?) при этом в бд хранить все еще в одной таблице
        public Task<string> TransitionState(User? currentState, Message message)
        {
            if (currentState == null)
            {
                return WithoutState(message);
            }

            switch (currentState.DialogState)
            {
                case DialogState.ProposedInputCity:
                    return ProposedInputCity(currentState, message);
                case DialogState.ProposedFoundedCity:
                    return ProposedFoundedCity(currentState, message);
                case DialogState.OfChoosingSubscribeType:
                    return OfChoosingSubscribeType(currentState, message);
                case DialogState.SubscribedToEverydayPushes:
                    throw new NotImplementedException();
                case DialogState.SubscribedToEverydayDoublePushes:
                    throw new NotImplementedException();
                case DialogState.SubscribedTriesToUnsubscribe:
                    throw new NotImplementedException();
                case DialogState.Unsubscribed:
                    throw new NotImplementedException();
                case DialogState.UnsubscribedTriesSubscribe:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentState.DialogState), currentState.DialogState, null);

            }
        }


        private async Task<string> ProposedInputCity(User currentState,
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
                var newState = new User
                {
                    PreviousDialogState = currentState.DialogState,
                    DialogState = DialogState.ProposedFoundedCity,
                    Id = currentState.Id,
                    CityId = city.Id,
                    StateChangeDate = DateTime.UtcNow
                };

                if (cityNotExistInDB)
                {
                    await _cityDao.Create(city)
                                  .ConfigureAwait(false);
                }
                await _dialogStateDao.Update(newState)
                                     .ConfigureAwait(false);

                return $"Ваш город {city.Address}?";
            }

            return "Город с таким названием не найден. Попробуйте написать точное название вашего города.";
        }

        private async Task<string> ProposedFoundedCity(User currentState,
                                                       Message message)
        {
            if (message.Text.Trim().ToLower() == "да")
            {
                var newState = new User
                {
                    Id = currentState.Id,
                    PreviousDialogState = currentState.DialogState,
                    DialogState = DialogState.OfChoosingSubscribeType,
                    CityId = currentState.CityId,
                    StateChangeDate = DateTime.UtcNow
                };
                await _dialogStateDao.Update(newState).ConfigureAwait(false);

                var city = await _citiesParserService.FindCity(currentState.CityId.Value)
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
                var newState = new User
                {
                    Id = currentState.Id,
                    PreviousDialogState = currentState.DialogState,
                    DialogState = DialogState.ProposedInputCity,
                    StateChangeDate = DateTime.UtcNow
                };
                await _dialogStateDao.Update(newState).ConfigureAwait(false);

                return "Возможно вы ввели неполное название города, попробуйте еще раз.";
            }
        }

        private async Task<string> OfChoosingSubscribeType(User currentState,
                                                           Message message)
        {
            switch (message.Text.Trim().ToLower())
            {
                case "обычная":
                    {
                        var newState = new User
                        {
                            Id = currentState.Id,
                            PreviousDialogState = currentState.DialogState,
                            DialogState = DialogState.SubscribedToEverydayPushes,
                            CityId = currentState.CityId,
                            StateChangeDate = DateTime.UtcNow
                        };
                        await _dialogStateDao.Update(newState).ConfigureAwait(false);

                        return
                            "Вы подписались на обычную рассылку. В случае успешного прогноза заката вам будет отправлено уведомление за час.";
                    }

                case "двойная":
                    {
                        var newState = new User
                        {
                            Id = currentState.Id,
                            PreviousDialogState = currentState.DialogState,
                            DialogState = DialogState.SubscribedToEverydayDoublePushes,
                            CityId = currentState.CityId,
                            StateChangeDate = DateTime.UtcNow
                        };
                        await _dialogStateDao.Update(newState).ConfigureAwait(false);

                        return
                            "Вы подписались на двойную рассылку. В случае успешного прогноза заката вам будет отправлено уведомление утром. " +
                            "Если вечером того же дня прогноз будет все еще успешен, вам отправится второе уведомление за час до заката.";
                    }

                default:
                    return "Введеный вариант подписки не распознан. Пожалуйста напишите один из вариантов: 'Обычная' или 'Двойная'";
            }
        }

        private async Task<string> WithoutState(Message message)
        {
            var newState = new User
            {
                DialogState = DialogState.ProposedInputCity,
                Id = message.From.Id,
                StateChangeDate = DateTime.UtcNow
            };
            await _dialogStateDao.Create(newState).ConfigureAwait(false);

            return "Пожалуйста введите название своего города (желательно точное название).";
        }
    }
}