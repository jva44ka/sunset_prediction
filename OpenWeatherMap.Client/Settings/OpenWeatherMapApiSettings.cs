namespace OpenWeatherMap.Client.Settings
{
    /// <summary>
    ///     Настройки Telegram Bot Api
    /// </summary>
    public class OpenWeatherMapApiSettings
    {
        /// <summary>
        ///     Корневой юрл Telegram Bot Api
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///     Личный токен
        /// </summary>
        public string ApiToken { get; set; }
    }
}
