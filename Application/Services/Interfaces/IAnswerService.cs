using Domain.Entities.Enums;
using TelegramApi.Client.Dtos;
using Application.Enums;

namespace Application.Services.Interfaces;

/// <summary>
///     Сервис генерации ответных сообщений
/// </summary>
public interface IAnswerService
{
    /// <summary>
    ///     Созщдать текст сообщения по типу сообщения
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public string GenerateAnswerText(
        AnswerMessageType messageType,
        params string[] args);

    /// <summary>
    ///     Создать клавиатуру по типу сообщения (если требуется)
    /// </summary>
    /// <param name="messageType"></param>
    /// <returns></returns>
    public ReplyKeyboardMarkupDto? GenerateKeyboard(AnswerMessageType messageType);
}