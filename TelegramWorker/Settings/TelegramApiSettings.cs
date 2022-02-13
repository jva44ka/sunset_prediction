namespace TelegramWorker.Settings
{
    /// <summary>
    ///     Настройки Telegram Bot Api
    /// </summary>
    public class TelegramApiSettings
    {
        /// <summary>
        ///     Корневой юрл Telegram Bot Api
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     Токен бота
        /// </summary>
        public string BotToken { get; set; }
    }
}
