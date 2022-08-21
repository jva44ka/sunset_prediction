using Domain.Entities.Enums;
using TelegramApi.Client.Dtos;

namespace Application.Services;

/// <summary>
///     Сервис генерации ответных сообщений
/// </summary>
public interface IAnswerService
{
    /// <summary>
    ///     Созщдать текст сообщения по типу сообщения
    /// </summary>
    /// <param name="messageType"></param>
    /// <returns></returns>
    public string GenerateAnswerText(AnswerMessageType messageType);

    /// <summary>
    ///     Создать клавиатуру по типу сообщения (если требуется)
    /// </summary>
    /// <param name="messageType"></param>
    /// <returns></returns>
    public ReplyKeyboardMarkupDto? GenerateKeyboard(AnswerMessageType messageType);
}