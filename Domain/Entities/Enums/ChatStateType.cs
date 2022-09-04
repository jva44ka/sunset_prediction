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
    FoundedProposedCity,

    /// <summary>
    ///     Город пользователя подтвержден. Ожидается выбор типа рассылки
    /// </summary>
    WithoutSubscribe,

    /// <summary>
    ///     Пользователь запросил новую подписку. Ожидается выбор конкретного типа подписки
    /// </summary>
    RequestedNewSubscribe,

    /// <summary>
    ///     Пользователь подписан на рассылку.
    /// </summary>
    Subscribed,

    /// <summary>
    ///     Пользователь запросил отписку. Ожидается выбор конкретного типа подписки
    /// </summary>
    RequestedUnsubscribe,
}