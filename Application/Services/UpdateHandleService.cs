using System;
using System.Threading.Tasks;
using Application.Mappers.Interfaces;
using Application.Services.Dto;
using Application.Services.Interfaces;
using DataAccess.Dao.Interfaces;
using TelegramApi.Client.Dtos;

namespace Application.Services
{
    public class UpdateHandleService : IUpdateHandleService
    {
        private readonly IUpdateDao _updateDao;
        private readonly IDialogStateService _dialogStateService;
        private readonly IAnswerService _answerService;
        private readonly IMapper<Domain.Entities.Update, UpdateDto> _updatesMapper;

        public UpdateHandleService(
            IUpdateDao updateDao,
            IDialogStateService dialogStateService,
            IAnswerService answerService,
            IMapper<Domain.Entities.Update, UpdateDto> updatesMapper)
        {
            _updateDao = updateDao;
            _dialogStateService = dialogStateService;
            _answerService = answerService;
            _updatesMapper = updatesMapper;
        }

        public async Task<long?> GetLastUpdateExternalId()
        {
            var lastUpdate = await _updateDao.GetLastUpdate();
            return lastUpdate?.ExternalId;
        }

        public async Task<HandleUpdateResult> HandleUpdate(UpdateDto update)
        {
            var updateDal = _updatesMapper.ToEntity(update);
            updateDal.HandleDate = DateTime.UtcNow;
            var creationResult = await _updateDao.Create(updateDal);

            if (creationResult == false)
            {
                throw new Exception($"Failed to create record in table \"updates\" with update_id: {update.UpdateId}");
            }

            var userId = update.Message.From.Id;
            var transitionResult = await _dialogStateService.TransitionState(userId, update.Message);
            //TODO: Рефакторинг параметров
            var messageText = _answerService.GenerateAnswerText(
                transitionResult.AnswerMessageType,
                transitionResult.CityAddress ?? string.Empty);
            var keyboard = _answerService.GenerateKeyboard(transitionResult.AnswerMessageType);

            return new HandleUpdateResult
            {
                ChatId = update.Message.Chat.Id, 
                MessageText = messageText,
                MessageKeyboard = keyboard
            };
        }
    }
}
