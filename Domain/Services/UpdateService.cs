using System;
using System.Threading.Tasks;
using DataAccess.DAL;
using DataAccess.DAO.Interfaces;
using Domain.Entities;
using Domain.Entities.TelegramApi;
using Domain.Mappers.Interfaces;
using Domain.Services.Dto;
using Domain.Services.Interfaces;
using User = Domain.Entities.User;

namespace Domain.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IUpdateDao _updateDao;
        private readonly IMapper<Update, UpdateDal> _updatesMapper;
        private readonly IDialogStateDao _dialogStateDao;
        private readonly IDialogStateService _dialogStateService;
        private readonly IMapper<User, UserDal> _dialogStateMapper;

        public UpdateService(IUpdateDao updateDao,
                             IMapper<Update, UpdateDal> updatesMapper,
                             IDialogStateDao dialogStateDao,
                             IDialogStateService dialogStateService,
                             IMapper<User, UserDal> dialogStateMapper)
        {
            _updateDao = updateDao;
            _updatesMapper = updatesMapper;
            _dialogStateDao = dialogStateDao;
            _dialogStateService = dialogStateService;
            _dialogStateMapper = dialogStateMapper;
        }

        public async Task<int?> GetLastUpdateId()
        {
            var lastUpdate = await _updateDao.GetLastUpdate().ConfigureAwait(false);
            return lastUpdate?.UpdateId;
        }

        public async Task<HandleUpdateResult> HandleUpdate(Update update)
        {
            var updateDal = _updatesMapper.ToDal(update);
            updateDal.HandleDate = DateTime.UtcNow;
            var creationResult = await _updateDao.Create(updateDal).ConfigureAwait(false);

            if (!creationResult)
            {
                throw new Exception($"Failed to create record in table \"updates\" with update_id: {update.UpdateId}");
            }

            var userId = update.Message.From.Id;
            var currentStateDal = await _dialogStateDao.GetStateByUserId(userId).ConfigureAwait(false);
            var currentState = _dialogStateMapper.ToEntity(currentStateDal);
            var resultMessage = await _dialogStateService.TransitionState(currentState, update.Message).ConfigureAwait(false);

            return new HandleUpdateResult
            {
                ChatId = update.Message.Chat.Id, 
                ResultMessageText = resultMessage
            };
        }
    }
}
