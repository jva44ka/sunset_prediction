using System;
using Domain.Entities.Enums;

namespace Domain.Entities;

public class Chat
{
    /// <summary>
    ///     Идентификатор чата
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Идентификатор чата в Telegram
    /// </summary>
    public long ExternalId { get; set; }

    /// <summary>
    ///     Предыдущее состояние чата 
    /// </summary>
    public ChatStateType? PreviousState { get; set; }

    /// <summary>
    ///     Текущее состояние чата
    /// </summary>
    public ChatStateType CurrentState { get; set; }

    /// <summary>
    ///     Дата последнего изменения состояния чата
    /// </summary>
    public DateTime StateChangedAt { get; set; }
}