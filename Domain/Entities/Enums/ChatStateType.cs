namespace Domain.Entities.Enums;

public enum ChatStateType : byte
{
    /// <summary>
    ///     Пользователь инициировал диалог. Ожидается ввод названия города
    /// </summary>
    ProposedInputCity = 1,

    /// <summary>
    ///     Распознан введенный пользователем город. Ожидается подтверждение от пользователя
    /// </summary>
    FoundedProposedCity = 2,

    /// <summary>
    ///     Город пользователя подтвержден. Ожидается выбор типа рассылки
    /// </summary>
    WithoutSubscribe = 3,

    /// <summary>
    ///     Пользователь запросил новую подписку. Ожидается выбор конкретного типа подписки
    /// </summary>
    RequestedNewSubscribe = 4,

    /// <summary>
    ///     Пользователь подписан только на закаты.
    /// </summary>
    SubscribedSunset = 5,

    /// <summary>
    ///     Пользователь подписан только на грозы.
    /// </summary>
    SubscribedLightning = 6,

    /// <summary>
    ///     Пользователь подписан на закаты и грозы.
    /// </summary>
    SubscribedSunsetAndLightning = 7,

    /// <summary>
    ///     Пользователь запросил отписку. Ожидается выбор конкретного типа подписки
    /// </summary>
    RequestedUnsubscribe = 8,
}