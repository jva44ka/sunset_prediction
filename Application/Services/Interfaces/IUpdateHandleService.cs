using System.Threading.Tasks;
using Application.Services.Dto;
using TelegramApi.Client.Dtos;

namespace Application.Services.Interfaces
{
    public interface IUpdateHandleService
    {
        public Task<long?> GetLastUpdateExternalId();

        /// <summary>
        ///     Обрабатывает <see cref="UpdateDto"/>
        /// </summary>
        /// <returns>Сообщение, которое нужно отправить пользователю в ответ</returns>
        public Task<HandleUpdateResult> HandleUpdate(UpdateDto update);
    }
}