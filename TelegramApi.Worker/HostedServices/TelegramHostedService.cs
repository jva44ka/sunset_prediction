using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TelegramApi.Worker.HostedServices
{
    public class TelegramBackgroundService : BackgroundService
    {
        private readonly ITelegramUpdatesRequesterService _telegramUpdatesRequesterService;
        private readonly ILogger<TelegramBackgroundService> _logger;

        public TelegramBackgroundService(
            ITelegramUpdatesRequesterService telegramUpdatesRequesterService,
            ILogger<TelegramBackgroundService> logger)
        {
            _telegramUpdatesRequesterService = telegramUpdatesRequesterService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Worker started at: {DateTimeOffset.UtcNow}");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");

                try
                {
                    await _telegramUpdatesRequesterService
                          .HandleNewUpdates(
                              stoppingToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    _logger.LogError(e.Message);
                }

                //TODO: Добавить умный вызов таймаута для хостедсервиса, который будет выставлять больший таймаут
                //TODO: если давно не было апдейтов и наоборот меньший/без таймаута при наличии нагрузки.
            }

            _logger.LogInformation($"Worker stopped at: {DateTimeOffset.UtcNow}");
        }
    }
}
