using DataAccess.DAL;
using Domain.Entities;
using Domain.Mappers.Interfaces;

namespace Domain.Mappers
{
    public class DialogStateMapper : IMapper<DialogState, DialogStateDal>
    {
        public DialogState? ToEntity(DialogStateDal? dal)
        {
            if (dal == null)
            {
                return null;
            }

            return new DialogState
            {
                UserId = dal.UserId,
                //TODO: Опасный каст, при различиях в номерах все поедет
                PreviousState = dal.PreviousState == null ? null : (DialogStateEnum)(byte)dal.PreviousState,
                State = (DialogStateEnum)(byte)dal.State,
                ProposedCityId = dal.ProposedCityId,
                StateChangeDate = dal.StateChangeDate
            };
        }

        public DialogStateDal? ToDal(DialogState? entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new DialogStateDal
            {
                UserId = entity.UserId,
                //TODO: Опасный каст, при различиях в номерах все поедет
                PreviousState = entity.PreviousState == null ? null : (DialogStateEnumDal)(byte)entity.PreviousState,
                State = (DialogStateEnumDal)(byte)entity.State,
                ProposedCityId = entity.ProposedCityId,
                StateChangeDate = entity.StateChangeDate
            };
        }
    }
}