using DataAccess.DAL;
using Domain.Entities;
using Domain.Mappers.Interfaces;

namespace Domain.Mappers
{
    public class DialogStateMapper : IMapper<User, UserDal>
    {
        public User? ToEntity(UserDal? dal)
        {
            if (dal == null)
            {
                return null;
            }

            return new User
            {
                Id = dal.Id,
                //TODO: Опасный каст, при различиях в номерах все поедет
                PreviousDialogState = dal.PreviousDialogState == null ? null : (DialogState)(byte)dal.PreviousDialogState,
                DialogState = (DialogState)(byte)dal.DialogState,
                CityId = dal.CityId,
                StateChangeDate = dal.StateChangeDate
            };
        }

        public UserDal? ToDal(User? entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new UserDal
            {
                Id = entity.Id,
                //TODO: Опасный каст, при различиях в номерах все поедет
                PreviousDialogState = entity.PreviousDialogState == null ? null : (DialogStateDal)(byte)entity.PreviousDialogState,
                DialogState = (DialogStateDal)(byte)entity.DialogState,
                CityId = entity.CityId,
                StateChangeDate = entity.StateChangeDate
            };
        }
    }
}