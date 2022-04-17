using Application.Mappers.Interfaces;

namespace Application.Mappers
{
    public class UpdateMapper : IMapper<Domain.Entities.Update, TelegramApi.Client.Entities.Update>
    {
        public Domain.Entities.Update? ToEntity(TelegramApi.Client.Entities.Update? dal)
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

        public TelegramApi.Client.Entities.Update? ToDto(Domain.Entities.Update? entity)
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
