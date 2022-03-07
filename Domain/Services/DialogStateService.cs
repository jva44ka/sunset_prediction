using System;
using System.Threading.Tasks;
using DataAccess.DAL;
using DataAccess.DAO.Interfaces;
using Domain.Entities;
using Domain.Entities.TelegramApi;
using Domain.Mappers.Interfaces;
using Domain.Services.Interfaces;

namespace Domain.Services
{

    public class DialogStateService : IDialogStateService
    {
        private readonly ICitiesParserService _citiesParserService;
        private readonly IDialogStateDao _dialogStateDao;
        private readonly ICityDao _cityDao;
        private readonly IMapper<DialogState, DialogStateDal> _dialogStateMapper;
        private readonly IMapper<City, CityDal> _cityMapper;

        public DialogStateService(ICitiesParserService citiesParserService,
                                  IDialogStateDao dialogStateDao,
                                  ICityDao cityDao,
                                  IMapper<DialogState, DialogStateDal> dialogStateMapper,
                                  IMapper<City, CityDal> cityMapper)
        {
            _citiesParserService = citiesParserService;
            _dialogStateDao = dialogStateDao;
            _cityDao = cityDao;
            _dialogStateMapper = dialogStateMapper;
            _cityMapper = cityMapper;
        }

        public Task<string> TransitionState(DialogState? currentState, Message message)
        {
            if (currentState == null)
            {
                return WithoutState(message);
            }

            switch (currentState.State)
            {
                case DialogStateEnum.ProposedInputCity:
                    return ProposedInputCity(currentState, message);
                case DialogStateEnum.ProposedFoundedCity:
                    return ProposedFoundedCity(currentState, message);
                case DialogStateEnum.OfChoosingSubscribeType:
                    return OfChoosingSubscribeType(currentState, message);
                case DialogStateEnum.SubscribedToEverydayPushes:
                    throw new NotImplementedException();
                case DialogStateEnum.SubscribedToEverydayDoublePushes:
                    throw new NotImplementedException();
                case DialogStateEnum.SubscribedTriesToUnsubscribe:
                    throw new NotImplementedException();
                case DialogStateEnum.Unsubscribed:
                    throw new NotImplementedException();
                case DialogStateEnum.UnsubscribedTriesSubscribe:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentState.State), currentState.State, null);

            }
        }


        private async Task<string> ProposedInputCity(DialogState currentState,
                                                     Message message)
        {
            //TODO: сначала искать по базе
            var city = await _citiesParserService.FindCity(message.Text).ConfigureAwait(false);
            if (city != null)
            {
                var newState = new DialogState
                {
                    PreviousState = currentState.State,
                    State = DialogStateEnum.ProposedFoundedCity,
                    UserId = currentState.UserId,
                    ProposedCityId = city.Id,
                    StateChangeDate = DateTime.UtcNow
                };
                var newStateDal = _dialogStateMapper.ToDal(newState);
                await _dialogStateDao.Update(newStateDal).ConfigureAwait(false);

                return $"Ваш город {city.Address}?";
            }
            else
            {
                return "Город с таким названием не найден. Попробуйте написать точное название вашего города.";
            }
        }

        private async Task<string> ProposedFoundedCity(DialogState currentState,
                                                       Message message)
        {
            if (message.Text.Trim().ToLower() == "да")
            {
                var newState = new DialogState
                {
                    UserId = currentState.UserId,
                    PreviousState = currentState.State,
                    State = DialogStateEnum.OfChoosingSubscribeType,
                    ProposedCityId = currentState.ProposedCityId,
                    StateChangeDate = DateTime.UtcNow
                };
                var newStateDal = _dialogStateMapper.ToDal(newState);
                await _dialogStateDao.Update(newStateDal).ConfigureAwait(false);

                var city = await _citiesParserService.FindCity(currentState.ProposedCityId.Value)
                                                     .ConfigureAwait(false);
                var cityInDb = await _cityDao.GetCityById(city.Id).ConfigureAwait(false);
                if (cityInDb == null)
                {
                    var cityDal = _cityMapper.ToDal(city);
                    await _cityDao.Create(cityDal).ConfigureAwait(false);
                }

                return "Выбирите и напишите вариант рассылки:\n" +
                       "1. 'Обычная' - бот присылает одно сообщение за час до заката при высокой вероятности заката \n" +
                       "2. 'Двойная' - бот присылает одно сообщение с утра, второе сообщение за час до заката при высокой вероятности заката";
            }
            else
            {
                var newState = new DialogState
                {
                    UserId = currentState.UserId,
                    PreviousState = currentState.State,
                    State = DialogStateEnum.ProposedInputCity,
                    StateChangeDate = DateTime.UtcNow
                };
                var newStateDal = _dialogStateMapper.ToDal(newState);
                await _dialogStateDao.Update(newStateDal).ConfigureAwait(false);

                return "Возможно вы ввели неполное название города, попробуйте еще раз.";
            }
        }

        private async Task<string> OfChoosingSubscribeType(DialogState currentState,
                                                           Message message)
        {
            switch (message.Text.Trim().ToLower())
            {
                case "обычная":
                    {
                        var newState = new DialogState
                        {
                            UserId = currentState.UserId,
                            PreviousState = currentState.State,
                            State = DialogStateEnum.SubscribedToEverydayPushes,
                            ProposedCityId = currentState.ProposedCityId,
                            StateChangeDate = DateTime.UtcNow
                        };
                        var newStateDal = _dialogStateMapper.ToDal(newState);
                        await _dialogStateDao.Update(newStateDal).ConfigureAwait(false);

                        return
                            "Вы подписались на обычную рассылку. В случае успешного прогноза заката вам будет отправлено уведомление за час.";
                    }

                case "двойная":
                    {
                        var newState = new DialogState
                        {
                            UserId = currentState.UserId,
                            PreviousState = currentState.State,
                            State = DialogStateEnum.SubscribedToEverydayDoublePushes,
                            ProposedCityId = currentState.ProposedCityId,
                            StateChangeDate = DateTime.UtcNow
                        };
                        var newStateDal = _dialogStateMapper.ToDal(newState);
                        await _dialogStateDao.Update(newStateDal).ConfigureAwait(false);

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
            var newState = new DialogState
            {
                State = DialogStateEnum.ProposedInputCity,
                UserId = message.From.Id,
                StateChangeDate = DateTime.UtcNow
            };
            var newStateDal = _dialogStateMapper.ToDal(newState);
            await _dialogStateDao.Create(newStateDal).ConfigureAwait(false);

            return "Пожалуйста введите название своего города (желательно точное название).";
        }
    }
}