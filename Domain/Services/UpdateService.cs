using System;
using System.Threading.Tasks;
using DataAccess.DAL;
using DataAccess.DAO.Interfaces;
using Domain.Entities;
using Domain.Mappers.Interfaces;
using Domain.Services.Interfaces;

namespace Domain.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IUpdateDao _updateDao;
        private readonly IMapper<Update, UpdateDal> _mapper;

        public UpdateService(IUpdateDao updateDao, 
                             IMapper<Update, UpdateDal> mapper)
        {
            _updateDao = updateDao;
            _mapper = mapper;
        }

        public async Task<int?> GetLastUpdateId()
        {
            var lastUpdate = await _updateDao.GetLastUpdate().ConfigureAwait(false);
            return lastUpdate?.UpdateId;
        }

        public async Task HandleUpdate(Update update)
        {
            var updateDal = _mapper.ToDal(update);
            updateDal.HandleDate = DateTime.UtcNow;
            var creationResult = await _updateDao.Create(updateDal).ConfigureAwait(false);

            if (!creationResult)
            {
                throw new Exception($"Failed to create record in table \"updates\" with update_id: {update.UpdateId}");
            }

            //TODO: реагируем на апдейт: подписываем юзер, спрашиваем город и т.д.
        }
    }
}
