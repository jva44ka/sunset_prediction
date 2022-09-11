using System;

namespace Domain.Entities.Enums;

[Flags]
public enum SubscribeType : byte
{
    None = 0,
    
    /// <summary>
    ///     Подписка на прогноз грозы
    /// </summary>
    Sunset = 1,

    /// <summary>
    ///     Подписка на красивый закат
    /// </summary>
    Lightning = 2
}