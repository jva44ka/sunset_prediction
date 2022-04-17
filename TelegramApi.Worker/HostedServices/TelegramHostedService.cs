using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TelegramApi.Client.Clients.Interfaces;
using TelegramApi.Client.DTO;
using TelegramApi.Client.Entities;
using TelegramApi.Client.Settings;

namespace TelegramApi.Worker.HostedServices
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
                            var updateHandleResults = await UpdatesHandle(responseDto.Result).ConfigureAwait(false);
                            await SendUpdateHandleResults(updateHandleResults, stoppingToken).ConfigureAwait(false);
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

        /// <summary>
        ///     Обработка обновлений
        /// </summary>
        /// <param name="updates"></param>
        /// <returns>
        ///     Словарь вида: <"id чата", "сообщение в результате обработки последнего апдейта в этом чате">
        /// </returns>
        private async Task<Dictionary<int, string>> UpdatesHandle(Update[] updates)
        {
            var tasks = updates.Select(u => _updateService.HandleUpdate(u));
            var handleResults = await Task.WhenAll(tasks);

            var allUniqueChatIds = handleResults.Select(hr => hr.ChatId).Distinct();

            // Формируем словарь вида: <"id чата", "сообщение в результате обработки последнего апдейта в этом чате">
            var chatIdResultMessagesDictionary = new Dictionary<int, string>();
            foreach (var chatId in allUniqueChatIds)
            {
                var lastMessageInChat = handleResults.Where(hr => hr.ChatId == chatId)
                                                     .OrderBy(hr => hr.RequestMessageId)
                                                     .Last()
                                                     .ResultMessageText;
                chatIdResultMessagesDictionary.Add(chatId, lastMessageInChat);
            }
            return chatIdResultMessagesDictionary;
        }

        private async Task SendUpdateHandleResults(Dictionary<int, string> updateHandleResults,
                                                   CancellationToken stoppingToken)
        {
            foreach (var chatId in updateHandleResults.Keys)
            {
                var sendMessageRequest = new TelegramSendMessageRequest
                {
                    ChatId = chatId,
                    Text = updateHandleResults[chatId],
                };
                //TODO: Добавить таймауты т.к. нельзя отправлять более 30 сообщений в секунду вроде бы
                await _telegramBotApiClient.SendMessage(sendMessageRequest, stoppingToken);
            }
        }
    }
}
