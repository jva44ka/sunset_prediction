using System.Threading.Tasks;
using Application.Services.Dto;
using TelegramApi.Client.Dtos;

namespace Application.Services.Interfaces
{
    public interface IUpdateHandleService
    {
        public Task<int?> GetLastUpdateId();

        /// <summary>
        ///     Обрабатывает <see cref="Update"/>
        /// </summary>
        /// <returns>Сообщение, которое нужно отправить пользователю в ответ</returns>
        public Task<HandleUpdateResult> HandleUpdate(Update update);
    }
}