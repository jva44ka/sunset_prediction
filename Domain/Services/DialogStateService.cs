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
        private readonly IMapper<DialogState, DialogStateDal> _mapper;

        public DialogStateService(ICitiesParserService citiesParserService,
                                  IDialogStateDao dialogStateDao,
                                  IMapper<DialogState, DialogStateDal> mapper)
        {
            _citiesParserService = citiesParserService;
            _dialogStateDao = dialogStateDao;
            _mapper = mapper;
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
                    throw new NotImplementedException();
                case DialogStateEnum.OfChoosingSubscribeType:
                    throw new NotImplementedException();
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
            var city = await _citiesParserService.FindCity(message.Text).ConfigureAwait(false);
            if (city != null)
            {
                var newState = new DialogState
                {
                    PreviousState = currentState.State,
                    State = DialogStateEnum.ProposedFoundedCity,
                    UserId = currentState.UserId,
                    ProposedCityId = city.Id
                };
                var newStateDal = _mapper.ToDal(newState);
                await _dialogStateDao.Update(newStateDal).ConfigureAwait(false);

                return $"Ваш город {city.Address}?";
            }
            else
            {
                return "Город с таким названием не найден. Попробуйте написать точное название вашего города.";
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
            var newStateDal = _mapper.ToDal(newState);
            await _dialogStateDao.Create(newStateDal).ConfigureAwait(false);

            return "Пожалуйста введите название своего города (желательно точное название).";
        }
    }
}