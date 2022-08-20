using Application.Mappers.Interfaces;
using TelegramApi.Client.Dtos;

namespace Application.Mappers
{
    public class UpdateMapper : IMapper<Domain.Entities.Update, Update>
    {
        public Domain.Entities.Update? ToEntity(Update? dal)
        {
            if (dal == null)
            {
                return null;
            }

            return new Domain.Entities.Update
            {
                UpdateId = dal.UpdateId
            };
        }

        public Update? ToDto(Domain.Entities.Update? entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new TelegramApi.Client.Entities.Update
            {
                UpdateId = entity.UpdateId
            };
        }
    }
}
