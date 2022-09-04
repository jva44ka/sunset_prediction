namespace Domain.Entities.Enums;

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
    ///     "Введеный вариант подписки не распознан. Пожалуйста напишите один " +
    ///     "из вариантов: 'Обычная' или 'Двойная'.",
    /// </summary>
    InputSubscribeNameWrong,

    /// <summary>
    ///     "Вы отписаны от всех рассылок. \n" +
    ///     "Если вы хотите подписаться на рассылку прогноза - от рассылки напишите 'Подписка'"
    /// </summary>
    Unsubscribed,

    /// <summary>
    ///     "Вы подписаны на рассылки на [грозы/закаты/закаты и грозы]. \n" +
    ///     "Если вы хотите отписаться от рассылки напишите 'Отписка'.",
    /// </summary>
    Subscribed,

    /// <summary>
    ///     "Чтобы подписаться на рассылку введите название рассылки.\n" +
    ///     "Вам доступны [грозы/закаты/закаты и грозы]. "
    /// </summary>
    RequestedNewSubscribe
}