using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TelegramApi.Client.Clients.Interfaces;
using TelegramApi.Client.Dtos;
using TelegramApi.Client.Settings;

namespace TelegramApi.Client.Clients
{
    public class TelegramBotApiClient : ITelegramBotApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TelegramApiSettings _telegramApiSettings;

        public TelegramBotApiClient(IHttpClientFactory httpClientFactory,
                                    IOptions<TelegramApiSettings> telegramApiSettingsOptions)
        {
            _httpClientFactory = httpClientFactory;
            _telegramApiSettings = telegramApiSettingsOptions.Value;
        }

        public Task<HttpResponseMessage> GetUpdates(long? lastHandledUpdateId, CancellationToken stoppingToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var getUpdatesUrl = GetUpdatesUrl(lastHandledUpdateId);
            return httpClient.GetAsync(getUpdatesUrl, stoppingToken);
        }
        
        public async Task<HttpResponseMessage> SendMessage(SendMessageRequest request, CancellationToken stoppingToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = SendMessageUrl();
            var body = request.ToJson();
            return await httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"), stoppingToken);
        }

        /// <summary>
        ///     Строит URL для запроса к апи-методу "getUpdates"
        /// </summary>
        private string GetUpdatesUrl(long? lastHandledUpdateId)
        {
            var getUpdatesUrl = $"{_telegramApiSettings.Url}/bot{_telegramApiSettings.BotToken}/getUpdates";
            getUpdatesUrl += $"?timeout={_telegramApiSettings.LongPoolingTimeoutSec}";
            if (lastHandledUpdateId != null)
            {
                getUpdatesUrl += $"&offset={lastHandledUpdateId.Value + 1}";
            }

            return getUpdatesUrl;
        }

        /// <summary>
        ///     Строит URL для запроса к апи-методу "sendMessage"
        /// </summary>
        private string SendMessageUrl()
        {
            return $"{_telegramApiSettings.Url}/bot{_telegramApiSettings.BotToken}/sendMessage";
        }
    }
}
