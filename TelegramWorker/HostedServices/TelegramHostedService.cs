using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TelegramWorker.DTO;
using TelegramWorker.Settings;

namespace TelegramWorker.HostedServices
{
    public class TelegramBackgroundService : BackgroundService
    {
        private readonly ILogger<TelegramBackgroundService> _logger;
        private readonly IUpdateService _updateService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<TelegramApiSettings> _telegramApiSettings;

        public TelegramBackgroundService(ILogger<TelegramBackgroundService> logger,
                                         IUpdateService updateService,
                                         IHttpClientFactory httpClientFactory,
                                         IOptions<TelegramApiSettings> telegramApiSettings)
        {
            _logger = logger;
            _updateService = updateService;
            _httpClientFactory = httpClientFactory;
            _telegramApiSettings = telegramApiSettings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Worker started at: {DateTimeOffset.UtcNow}");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    var httpClient = _httpClientFactory.CreateClient();
                    var getUpdatesUrl = await GetUpdatesUrl().ConfigureAwait(false);
                    var response = await httpClient.GetAsync(getUpdatesUrl, stoppingToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContentString = await response.Content.ReadAsStringAsync(stoppingToken);
                        var updatesResult = JsonConvert.DeserializeObject<TelegramGetUpdatesResult>(responseContentString);

                        if (updatesResult.Ok && updatesResult.Result.Length > 0)
                        {
                            var messageResult = string.Empty;

                            //TODO: распаралелить
                            foreach (var update in updatesResult.Result)
                            {
                                messageResult = await _updateService.HandleUpdate(update);
                            }

                            await SendMessage(messageResult, updatesResult.Result.Last().Message.Chat.Id);
                        }
                    }
                    else
                    {
                        _logger.LogError($"Error on telegram updates receiving request\t:{DateTime.UtcNow}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    _logger.LogError(e.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(_telegramApiSettings.Value.LongPoolingTimeoutSec), stoppingToken);
            }

            _logger.LogInformation($"Worker stopped at: {DateTimeOffset.UtcNow}");
        }

        private async Task SendMessage(string messageText, int chatId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = SendMessageUrl();
            var body = $@"
{{
    ""chat_id"": ""{chatId}"",
    ""text"": ""{messageText}""
}}
";
            await httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
        }

        /// <summary>
        ///     Строит URL для запроса к апи-методу "getUpdates"
        /// </summary>
        private async Task<string> GetUpdatesUrl()
        {
            var lastHandledUpdateId = await _updateService.GetLastUpdateId()
                                                          .ConfigureAwait(false);
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
