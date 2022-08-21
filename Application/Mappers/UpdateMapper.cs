using Application.Mappers.Interfaces;
using TelegramApi.Client.Dtos;

namespace Application.Mappers
{
    public class UpdateMapper : IMapper<Domain.Entities.Update, UpdateDto>
    {
        public Domain.Entities.Update? ToEntity(UpdateDto? dal)
        {
            if (dal == null)
            {
                return null;
            }

            return new Domain.Entities.Update
            {
                ExternalId = dal.UpdateId
            };
        }

        public UpdateDto? ToDto(Domain.Entities.Update? entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new UpdateDto
            {
                UpdateId = entity.ExternalId
            };
        }
    }
}
