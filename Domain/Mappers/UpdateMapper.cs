using DataAccess.DAL;
using Domain.Entities;
using Domain.Mappers.Interfaces;

namespace Domain.Mappers
{
    public class UpdateMapper : IMapper<Update, UpdateDal>
    {
        public Update ToEntity(UpdateDal dal)
        {
            return new Update
            {
                UpdateId = dal.UpdateId
            };
        }

        public UpdateDal ToDal(Update entity)
        {
            return new UpdateDal
            {
                UpdateId = entity.UpdateId
            };
        }
    }
}
