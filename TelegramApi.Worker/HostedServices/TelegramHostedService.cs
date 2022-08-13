using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Services.Dto;
using Application.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TelegramApi.Client.Clients.Interfaces;
using TelegramApi.Client.DTO;
using TelegramApi.Client.Entities;

namespace TelegramApi.Worker.HostedServices
{
    public class TelegramBackgroundService : BackgroundService
    {
        private readonly ILogger<TelegramBackgroundService> _logger;
        private readonly IUpdateService _updateService;
        private readonly ITelegramBotApiClient _telegramBotApiClient;

        public TelegramBackgroundService(ILogger<TelegramBackgroundService> logger,
                                         IUpdateService updateService,
                                         ITelegramBotApiClient telegramBotApiClient)
        {
            _logger = logger;
            _updateService = updateService;
            _telegramBotApiClient = telegramBotApiClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Worker started at: {DateTimeOffset.UtcNow}");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    var lastHandledUpdateId = await _updateService.GetLastUpdateId();
                    var response = await _telegramBotApiClient.GetUpdates(lastHandledUpdateId, stoppingToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContentString = await response.Content.ReadAsStringAsync(stoppingToken);
                        var responseDto = JsonConvert.DeserializeObject<TelegramGetUpdatesResult>(responseContentString);

                        if (responseDto.Ok && responseDto.Result.Length > 0)
                        {
                            var updateHandleResults = await UpdatesHandle(responseDto.Result);
                            await SendUpdateHandleResults(updateHandleResults, stoppingToken);
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

                //await Task.Delay(TimeSpan.FromSeconds(_telegramApiSettings.Value.LongPoolingTimeoutSec), stoppingToken);
            }

            _logger.LogInformation($"Worker stopped at: {DateTimeOffset.UtcNow}");
        }

        /// <summary>
        ///     Обработка обновлений
        /// </summary>
        /// <param name="updates"></param>
        private async Task<List<HandleUpdateResult>> UpdatesHandle(Update[] updates)
        {
            var tasks = updates.Select(u => _updateService.HandleUpdate(u));
            var handleResults = await Task.WhenAll(tasks);

            var allUniqueChatIds = handleResults.Select(hr => hr.ChatId).Distinct();

            //Вырезаем лишние результаты обработки, когда для одного чата было несколько
            //апдейтов => несколько результатов, оставляем результат обработки последнего апдейта
            var handleUpdateResults = new List<HandleUpdateResult>();
            foreach (var chatId in allUniqueChatIds)
            {
                var lastMessageInChat = handleResults.Where(hr => hr.ChatId == chatId)
                                                     .OrderBy(hr => hr.RequestMessageId)
                                                     .Last();
                handleUpdateResults.Add(lastMessageInChat);
            }
            return handleUpdateResults;
        }

        private async Task SendUpdateHandleResults(List<HandleUpdateResult> updateHandleResults,
                                                   CancellationToken stoppingToken)
        {
            foreach (var handleUpdateResult in updateHandleResults)
            {
                var sendMessageRequest = new TelegramSendMessageRequest
                {
                    ChatId = handleUpdateResult.ChatId,
                    Text = handleUpdateResult.MessageText,
                    ReplyMarkup = handleUpdateResult.MessageKeyboard
                };
                //TODO: Добавить таймауты т.к. нельзя отправлять более 30 сообщений в секунду вроде бы
                await _telegramBotApiClient.SendMessage(sendMessageRequest, stoppingToken);
            }
        }
    }
}
