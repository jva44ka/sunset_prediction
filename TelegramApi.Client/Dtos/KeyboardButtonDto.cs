using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos;

/// <summary>
///     Этот объект представляет одну кнопку в клавиатуре ответа.
///     Для обычных текстовых кнопок этот объект может быть заменён на строку, содержащую текст на кнопке.
/// </summary>
public class KeyboardButtonDto
{
    /// <summary>
    ///     Текст на кнопке. Если ни одно из опциональных полей не использовано, то при нажатии на кнопку
    ///     этот текст будет отправлен боту как простое сообщение.
    /// </summary>
    [JsonProperty("text")]
    public string Text { get; set; } = default!;

    /// <summary>
    ///     Опционально. Если значение True, то при нажатии на кнопку боту отправится контакт пользователя
    ///     с его номером телефона. Доступно только в чатах с ботом.
    /// </summary>
    [JsonProperty("request_contact")]
    public bool? RequestContact { get; set; }

    /// <summary>
    ///     Опционально. Если значение True, то при нажатии на кнопку боту отправится местоположение пользователя.
    ///     Доступно только в чатах с ботом.
    /// </summary>
    [JsonProperty("request_location")]
    public bool? RequestLocation { get; set; }
}