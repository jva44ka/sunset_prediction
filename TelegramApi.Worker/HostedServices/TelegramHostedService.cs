using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TelegramApi.Worker.Services.Interfaces;

namespace TelegramApi.Worker.HostedServices
{
    public class TelegramBackgroundService : BackgroundService
    {
        private readonly ITelegramRequesterService _telegramRequesterService;
        private readonly ILogger<TelegramBackgroundService> _logger;

        public TelegramBackgroundService(
            ITelegramRequesterService telegramRequesterService,
            ILogger<TelegramBackgroundService> logger)
        {
            _telegramRequesterService = telegramRequesterService;
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
                    await _telegramRequesterService
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
