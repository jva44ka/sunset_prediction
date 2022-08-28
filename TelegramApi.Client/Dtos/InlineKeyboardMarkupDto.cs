using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos;

/// <summary>
///     Этот объект представляет встроенную клавиатуру, которая появляется под соответствующим сообщением.
/// </summary>
public class InlineKeyboardMarkupDto
{
    /// <summary>
    ///     Массив строк, каждая из которых является массивом объектов InlineKeyboardButtonDto.
    /// </summary>
    [JsonProperty("inline_keyboard")]
    public InlineKeyboardButtonDto[] InlineKeyboard { get; set; } = default!;
}