using System;
using System.Threading.Tasks;
using Application.Mappers.Interfaces;
using Application.Services.Dto;
using Application.Services.Interfaces;
using DataAccess.DAO.Interfaces;

namespace Application.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IUpdateDao _updateDao;
        private readonly IMapper<Domain.Entities.Update, TelegramApi.Client.Entities.Update> _updatesMapper;
        private readonly IUserDao _userDao;
        private readonly IDialogStateService _dialogStateService;

        public UpdateService(IUpdateDao updateDao,
                             IMapper<Domain.Entities.Update, TelegramApi.Client.Entities.Update> updatesMapper,
                             IUserDao userDao,
                             IDialogStateService dialogStateService)
        {
            _updateDao = updateDao;
            _updatesMapper = updatesMapper;
            _userDao = userDao;
            _dialogStateService = dialogStateService;
        }

        public async Task<int?> GetLastUpdateId()
        {
            var lastUpdate = await _updateDao.GetLastUpdate().ConfigureAwait(false);
            return lastUpdate?.UpdateId;
        }

        public async Task<HandleUpdateResult> HandleUpdate(TelegramApi.Client.Entities.Update update)
        {
            var updateDal = _updatesMapper.ToEntity(update);
            updateDal.HandleDate = DateTime.UtcNow;
            var creationResult = await _updateDao.Create(updateDal)
                                                 .ConfigureAwait(false);

            if (!creationResult)
            {
                throw new Exception($"Failed to create record in table \"updates\" with update_id: {update.UpdateId}");
            }

            var userId = update.Message.From.Id;
            var currentState = await _userDao.GetStateByUserId(userId)
                                             .ConfigureAwait(false);
            var resultMessage = await _dialogStateService.TransitionState(currentState, update.Message)
                                                         .ConfigureAwait(false);

            return new HandleUpdateResult
            {
                ChatId = update.Message.Chat.Id, 
                ResultMessageText = resultMessage
            };
        }
    }
}
