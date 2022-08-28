using System;
using System.Linq;
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

        public async Task<HandleUpdateResult> HandleUpdate(UpdateDto updateDto)
        {
            var update = _updatesMapper.ToEntity(updateDto)!;
            update.HandledAt = DateTime.UtcNow;
            var creationResult = await _updateDao.Create(update);

            if (creationResult == false)
            {
                throw new Exception($"Failed to create record in table \"updates\" with update_id: {updateDto.UpdateId}");
            }

            var userId = updateDto.Message.From.Id;
            var transitionResult = await _dialogStateService.TransitionState(
                updateDto.Message);
            //TODO: Рефакторинг параметров
            var messageText = _answerService.GenerateAnswerText(
                transitionResult.AnswerMessageType,
                transitionResult.AnswerMessageArgs);
            var keyboard = _answerService.GenerateKeyboard(transitionResult.AnswerMessageType);

            return new HandleUpdateResult
            {
                ChatId = updateDto.Message.Chat.Id,
                MessageText = messageText,
                MessageKeyboard = keyboard
            };
        }
    }
}
