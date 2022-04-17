namespace Domain.Entities.Enums
{
    public enum DialogState : byte
    {
        /// <summary>
        ///     Пользователь инициировал диалог. Ожидается ввод названия города
        /// </summary>
        /// <remarks>
        ///     Выход:
        ///     <list type="bullet">
        ///         <item><see cref="ProposedInputCity"/></item>
        ///         <item><see cref="ProposedFoundedCity"/></item>
        ///     </list>
        /// </remarks>
        ProposedInputCity = 0,

        /// <summary>
        ///     Распознан введенный пользователем город. Ожидается подтверждение от пользователя
        /// </summary>
        /// <remarks>
        ///     Вход:
        ///     <list type="bullet">
        ///         <item><see cref="ProposedInputCity"/></item>
        ///     </list>
        ///
        ///     Выход:
        ///     <list type="bullet">
        ///         <item><see cref="ProposedInputCity"/></item>
        ///         <item><see cref="OfChoosingSubscribeType"/></item>
        ///     </list>
        /// </remarks>
        ProposedFoundedCity = 1,

        /// <summary>
        ///     Город пользователя подтвержден. Ожидается выбор типа рассылки
        /// </summary>
        /// <remarks>
        ///     Вход:
        ///     <list type="bullet">
        ///         <item><see cref="ProposedFoundedCity"/></item>
        ///         <item><see cref="UnsubscribedTriesSubscribe"/></item>
        ///     </list>
        ///
        ///     Выход:
        ///     <list type="bullet">
        ///         <item><see cref="SubscribedToEverydayPushes"/></item>
        ///         <item><see cref="SubscribedToEverydayDoublePushes"/></item>
        ///     </list>
        /// </remarks>
        OfChoosingSubscribeType = 2,

        /// <summary>
        ///     Выбрана опция рассылки каждый день за час до заката
        /// </summary>
        /// <remarks>
        ///     Вход:
        ///     <list type="bullet">
        ///         <item><see cref="OfChoosingSubscribeType"/></item>
        ///     </list>
        ///
        ///     Выход:
        ///     <list type="bullet">
        ///         <item><see cref="SubscribedTriesToUnsubscribe"/></item>
        ///         <item><see cref="OfChoosingSubscribeType"/></item>
        ///     </list>
        /// </remarks>
        SubscribedToEverydayPushes = 3,

        /// <summary>
        ///     Выбрана опция рассылки каждый день за час до заката и с утра
        /// </summary>
        /// <remarks>
        ///     Вход:
        ///     <list type="bullet">
        ///         <item><see cref="OfChoosingSubscribeType"/></item>
        ///     </list>
        ///
        ///     Выход:
        ///     <list type="bullet">
        ///         <item><see cref="SubscribedTriesToUnsubscribe"/></item>
        ///         <item><see cref="OfChoosingSubscribeType"/></item>
        ///     </list>
        /// </remarks>
        SubscribedToEverydayDoublePushes = 4,

        /// <summary>
        ///     Пользователь инициировал отписку от рассылки. Ожидается подтверждение,
        ///     иначе возвращаемся к предыдущему состоянию
        /// </summary>
        /// <remarks>
        ///     Вход:
        ///     <list type="bullet">
        ///         <item><see cref="SubscribedToEverydayPushes"/></item>
        ///         <item><see cref="SubscribedToEverydayDoublePushes"/></item>
        ///     </list>
        ///
        ///     Выход:
        ///     <list type="bullet">
        ///         <item><see cref="Unsubscribed"/></item>
        ///         <item><see cref="SubscribedToEverydayPushes"/></item>
        ///         <item><see cref="SubscribedToEverydayDoublePushes"/></item>
        ///     </list>
        /// </remarks>
        SubscribedTriesToUnsubscribe = 5,


        /// <summary>
        ///     Пользователь отписан от рассылки. Ожидается активность (любое сообщение в чат)
        /// </summary>
        /// <remarks>
        ///     Вход:
        ///     <list type="bullet">
        ///         <item><see cref="SubscribedTriesToUnsubscribe"/></item>
        ///     </list>
        ///
        ///     Выход:
        ///     <list type="bullet">
        ///         <item><see cref="UnsubscribedTriesSubscribe"/></item>
        ///     </list>
        /// </remarks>
        Unsubscribed = 10,

        /// <summary>
        ///     Пользователь отписан проявил активность в чате, ожидается подтверждение
        ///     повторной подписки иначе возвращаемся к предыдущему состоянию
        /// </summary>
        /// <remarks>
        ///     Вход:
        ///     <list type="bullet">
        ///         <item><see cref="Unsubscribed"/></item>
        ///     </list>
        ///
        ///     Выход:
        ///     <list type="bullet">
        ///         <item><see cref="OfChoosingSubscribeType"/></item>
        ///         <item><see cref="Unsubscribed"/></item>
        ///     </list>
        /// </remarks>
        UnsubscribedTriesSubscribe = 11
    }
}