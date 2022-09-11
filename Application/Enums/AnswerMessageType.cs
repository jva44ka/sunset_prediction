namespace Application.Enums;

public enum AnswerMessageType : byte
{
    /// <summary>
    ///     "Пожалуйста введите название своего города (желательно точное название)."
    /// </summary>
    ProposedInputCity = 1,

    /// <summary>
    ///     "Ваш город {cityAddress}"?
    /// </summary>
    ProposedFoundedCityName,

    /// <summary>
    ///     "Город с таким названием не найден. Попробуйте написать точное название вашего города."
    /// </summary>
    CityNameNotFound,

    /// <summary>
    ///     "Возможно вы ввели неполное название города, попробуйте еще раз."
    /// </summary>
    ProposedCityNameWrong,

    /// <summary>
    ///     "Введеный вариант подписки не распознан.
    /// </summary>
    InputSubscribeNameWrong,

    /// <summary>
    ///     "Вы отписаны от всех рассылок. \n" +
    ///     "Если вы хотите подписаться на рассылку прогноза - от рассылки напишите 'Подписка'"
    /// </summary>
    Unsubscribed,

    /// <summary>
    ///     "Вы подписаны на рассылки закатов. \n" +
    ///     "Если вы хотите отписаться от рассылки напишите 'Отписка'.",
    /// </summary>
    SubscribedToSunset,
    
    /// <summary>
    ///     "Вы подписаны на рассылки гроз. \n" +
    ///     "Если вы хотите отписаться от рассылки напишите 'Отписка'.",
    /// </summary>
    SubscribedToLightning,

    /// <summary>
    ///     "Вы подписаны на рассылки закатов и гроз. \n" +
    ///     "Если вы хотите отписаться от рассылки напишите 'Отписка'.",
    /// </summary>
    SubscribedToSunsetAndLightning,

    /// <summary>
    ///     "Чтобы подписаться на рассылку введите название рассылки.\n" +
    ///     "Вам доступны [грозы/закаты/закаты и грозы]."
    /// </summary>
    RequestedNewSubscribeWithoutSubscribes,
    
    /// <summary>
    ///     "Чтобы подписаться на рассылку введите название рассылки.\n" +
    ///     "Вам доступны [грозы]."
    /// </summary>
    RequestedNewSubscribeWithSunsetSubscribe,
    
    /// <summary>
    ///     "Чтобы подписаться на рассылку введите название рассылки.\n" +
    ///     "Вам доступны [закаты]."
    /// </summary>
    RequestedNewSubscribeWithLightningSubscribed,

    /// <summary>
    ///     "Чтобы отписаться от рассылки введите название рассылки.\n" +
    ///     "Вы подписаны на доступны [грозы/закаты/закаты и грозы]."
    /// </summary>
    RequestedUnsubscribeWithSunsetAndLightningSubscribes,

    /// <summary>
    ///     "Чтобы отписаться от рассылки введите название рассылки.\n" +
    ///     "Вы подписаны на доступны [закаты]."
    /// </summary>
    RequestedUnsubscribeWithSunsetSubscribed,

    /// <summary>
    ///     "Чтобы отписаться от рассылки введите название рассылки.\n" +
    ///     "Вы подписаны на доступны [грозы]."
    /// </summary>
    RequestedUnsubscribeWithLightningSubscribe
}