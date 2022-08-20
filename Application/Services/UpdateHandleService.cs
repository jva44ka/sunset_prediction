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
        private readonly IMapper<Domain.Entities.Update, Update> _updatesMapper;

        public UpdateHandleService(
            IUpdateDao updateDao,
            IDialogStateService dialogStateService,
            IMapper<Domain.Entities.Update, Update> updatesMapper)
        {
            _updateDao = updateDao;
            _dialogStateService = dialogStateService;
            _updatesMapper = updatesMapper;
        }

        public async Task<int?> GetLastUpdateId()
        {
            var lastUpdate = await _updateDao.GetLastUpdate();
            return lastUpdate?.UpdateId;
        }

        public async Task<HandleUpdateResult> HandleUpdate(Update update)
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
            var resultKeyboard = _dialogStateService.BuildKeyboard(transitionResult.NewState);

            return new HandleUpdateResult
            {
                ChatId = update.Message.Chat.Id, 
                MessageText = transitionResult.Message,
                MessageKeyboard = resultKeyboard
            };
        }
    }
}
