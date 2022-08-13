using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TelegramApi.Client.Settings;

namespace TelegramApi.Worker.HostedServices
{
    public class TelegramBackgroundService : BackgroundService
    {
        private readonly ILogger<TelegramBackgroundService> _logger;
        private readonly ITelegramUpdatesRequesterService _telegramUpdatesRequesterService;
        private readonly TelegramApiSettings _telegramApiSettings;

        public TelegramBackgroundService(
            ILogger<TelegramBackgroundService> logger,
            ITelegramUpdatesRequesterService telegramUpdatesRequesterService,
            IOptions<TelegramApiSettings> telegramApiSettingsOptions)
        {
            _logger = logger;
            _telegramUpdatesRequesterService = telegramUpdatesRequesterService;
            _telegramApiSettings = telegramApiSettingsOptions.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Worker started at: {DateTimeOffset.UtcNow}");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    await _telegramUpdatesRequesterService.HandleNewUpdates(stoppingToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    _logger.LogError(e.Message);
                }

                //TODO: Добавить умный вызов умного таймаута для хостедсервиса, который будет выставлять больший таймаут
                //TODO: если давно не было апдейтов и наоборот меньший/без таймаута при наличии нагрузки.
                await Task.Delay(TimeSpan.FromSeconds(_telegramApiSettings.LongPoolingTimeoutSec), stoppingToken);
            }

            _logger.LogInformation($"Worker stopped at: {DateTimeOffset.UtcNow}");
        }
    }
}
