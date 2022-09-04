using Domain.Entities.Enums;

namespace Domain.Entities;

public class User
{
    /// <summary>
    ///     Идентификатор пользователя
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Идентификатор пользователя в Telegram
    /// </summary>
    public long ExternalId { get; set; }

    /// <summary>
    ///     Идентификатор города, который выбрал пользователь
    /// </summary>
    public int? CityId { get; set; }

    /// <summary>
    ///     Имя пользователя в Telegram
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    ///     Фамилия пользователя в Telegram
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    ///     Никнейм пользователя в Telegram
    /// </summary>
    public string UserName { get; set; } = default!;
    
    public SubscribeType? SubscribeType { get; set; }

    /// <summary>
    ///     Идентификатор чата с пользователем
    /// </summary>
    public int ChatId { get; set; }
}