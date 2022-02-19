using DataAccess.DAL;
using Domain.Entities.TelegramApi;
using Domain.Mappers.Interfaces;

namespace Domain.Mappers
{
    public class UpdateMapper : IMapper<Update, UpdateDal>
    {
        public Update? ToEntity(UpdateDal? dal)
        {
            if (dal == null)
            {
                return null;
            }

            return new Update
            {
                UpdateId = dal.UpdateId
            };
        }

        public UpdateDal? ToDal(Update? entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new UpdateDal
            {
                UpdateId = entity.UpdateId
            };
        }
    }
}
