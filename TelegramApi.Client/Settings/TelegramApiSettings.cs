namespace TelegramApi.Client.Settings
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

        /// <summary>
        ///     Таймаут опроса новых обновлений у бота в секундах
        /// </summary>
        public int LongPoolingTimeoutSec { get; set; }
    }
}
