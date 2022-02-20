using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TelegramWorker.Clients.Interfaces;
using TelegramWorker.DTO;
using TelegramWorker.Settings;

namespace TelegramWorker.Clients
{
    public class TelegramBotApiClient : ITelegramBotApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<TelegramApiSettings> _telegramApiSettings;

        public TelegramBotApiClient(IHttpClientFactory httpClientFactory,
                                    IOptions<TelegramApiSettings> telegramApiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _telegramApiSettings = telegramApiSettings;
        }

        public Task<HttpResponseMessage> GetUpdates(int? lastHandledUpdateId, CancellationToken stoppingToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var getUpdatesUrl = GetUpdatesUrl(lastHandledUpdateId);
            return httpClient.GetAsync(getUpdatesUrl, stoppingToken);
        }
        
        public async Task<HttpResponseMessage> SendMessage(TelegramSendMessageRequest request, CancellationToken stoppingToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = SendMessageUrl();
            var body = request.ToJson();
            return await httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"), stoppingToken);
        }

        /// <summary>
        ///     Строит URL для запроса к апи-методу "getUpdates"
        /// </summary>
        private string GetUpdatesUrl(int? lastHandledUpdateId)
        {
            var getUpdatesUrl = $"{_telegramApiSettings.Value.Url}/bot{_telegramApiSettings.Value.BotToken}/getUpdates";
            getUpdatesUrl += $"?timeout={_telegramApiSettings.Value.LongPoolingTimeoutSec}";
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
            return $"{_telegramApiSettings.Value.Url}/bot{_telegramApiSettings.Value.BotToken}/sendMessage";
        }
    }
}
