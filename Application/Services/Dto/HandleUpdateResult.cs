using TelegramApi.Client.Dtos;

namespace Application.Services.Dto;

public class HandleUpdateResult
{
    /// <summary>
    ///     Идентификатор чата, с которого пришел апдейт
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    ///     Идентификатор сообщения, по которому отправился нам апдейт
    /// </summary>
    public int RequestMessageId { get; set; }

    /// <summary>
    ///     Сообщение от бота, которое нужно отправить как результат обработки апдейта
    /// </summary>
    public string MessageText { get; set; } = default!;

    /// <summary>
    ///     Клавиатура для последующих вариантов ответов на сообщение
    /// </summary>
    public ReplyKeyboardMarkupDto? MessageKeyboard { get; set; }
}