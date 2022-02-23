using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.TelegramApi;
using Domain.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TelegramWorker.Clients.Interfaces;
using TelegramWorker.DTO;
using TelegramWorker.Settings;

namespace TelegramWorker.HostedServices
{
    public class TelegramBackgroundService : BackgroundService
    {
        private readonly ILogger<TelegramBackgroundService> _logger;
        private readonly IUpdateService _updateService;
        private readonly ITelegramBotApiClient _telegramBotApiClient;
        private readonly IOptions<TelegramApiSettings> _telegramApiSettings;

        public TelegramBackgroundService(ILogger<TelegramBackgroundService> logger,
                                         IUpdateService updateService,
                                         ITelegramBotApiClient telegramBotApiClient,
                                         IOptions<TelegramApiSettings> telegramApiSettings)
        {
            _logger = logger;
            _updateService = updateService;
            _telegramBotApiClient = telegramBotApiClient;
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
                    var lastHandledUpdateId = await _updateService.GetLastUpdateId()
                                                                  .ConfigureAwait(false);
                    var response = await _telegramBotApiClient.GetUpdates(lastHandledUpdateId, stoppingToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContentString = await response.Content.ReadAsStringAsync(stoppingToken);
                        var responseDto = JsonConvert.DeserializeObject<TelegramGetUpdatesResult>(responseContentString);

                        if (responseDto.Ok && responseDto.Result.Length > 0)
                        {
                            //TODO: Баг: если прийдут апдейты с разных чатов, отправится все равно одно сообщение последнему апдейту
                            var lastUpdateResultMessage = await UpdatesHandle(responseDto.Result);

                            var sendMessageRequest = new TelegramSendMessageRequest
                            {
                                Text = lastUpdateResultMessage,
                                ChatId = responseDto.Result.Last().Message.Chat.Id
                            };
                            await _telegramBotApiClient.SendMessage(sendMessageRequest, stoppingToken);
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

        private async Task<string> UpdatesHandle(Update[] updates)
        {
            var tasks = updates.Select(u => _updateService.HandleUpdate(u));
            var tasksResults = await Task.WhenAll(tasks);
            return tasksResults.Last();
        }
    }
}
