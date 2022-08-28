using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos;

/// <summary>
///     Этот объект представляет контакт с номером телефона.
/// </summary>
public class ContactDto
{
    /// <summary>
    ///     Номер телефона
    /// </summary>
    [JsonProperty("phone_number")]
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    ///     Имя
    /// </summary>
    [JsonProperty("first_name")]
    public string FirstName { get; set; } = default!;

    /// <summary>
    ///     Опционально. Фамилия
    /// </summary>
    [JsonProperty("last_name")]
    public string? LastName { get; set; }

    /// <summary>
    ///     Опционально. Идентификатор пользователя в Telegram
    /// </summary>
    [JsonProperty("user_id")]
    public int? UserId { get; set; }
}