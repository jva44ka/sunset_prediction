using System.Threading.Tasks;
using Application.Services.Dto;
using TelegramApi.Client.Dtos;

namespace Application.Services.Interfaces;

/// <summary>
///     Сервис-таблица переходов состояний чата
/// </summary>
public interface IChatStateService
{
    /// <summary>
    ///     Переход на новое состояние чата в зависимости
    ///     от присланного сообщения и предыдущего состояния
    /// </summary>
    /// <returns>Сообщение в ответ пользователю</returns>
    Task<AnswerDto> Transit(
        MessageDto messageDto);
}