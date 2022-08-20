using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Services.Dto;
using Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TelegramApi.Client.Clients.Interfaces;
using TelegramApi.Client.Dtos;

namespace Application.Services
{
    public class TelegramUpdatesRequesterService : ITelegramUpdatesRequesterService
    {
        private readonly IUpdateHandleService _updateHandleService;
        private readonly ITelegramBotApiClient _telegramBotApiClient;
        private readonly ILogger<TelegramUpdatesRequesterService> _logger;

        public TelegramUpdatesRequesterService(
            IUpdateHandleService updateHandleService,
            ITelegramBotApiClient telegramBotApiClient,
            ILogger<TelegramUpdatesRequesterService> logger)
        {
            _updateHandleService = updateHandleService;
            _telegramBotApiClient = telegramBotApiClient;
            _logger = logger;
        }
        
        public async Task HandleNewUpdates(CancellationToken cancellationToken)
        {
            var lastHandledUpdateId = await _updateHandleService.GetLastUpdateId();
            var response = await _telegramBotApiClient.GetUpdates(lastHandledUpdateId, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var responseContentString = await response.Content.ReadAsStringAsync(cancellationToken);
                var newUpdatesResponse = JsonConvert.DeserializeObject<GetUpdatesResult>(responseContentString);

                if (newUpdatesResponse.Ok && newUpdatesResponse.Result.Length > 0)
                {
                    var updateHandleResults = await UpdatesHandle(newUpdatesResponse.Result);
                    await SendUpdateHandleResults(updateHandleResults, cancellationToken);
                }
            }
            else
            {
                _logger.LogError($"Error on telegram updates receiving request\t:{DateTime.UtcNow}");
            }
        }

        /// <summary>
        ///     Обработка обновлений
        /// </summary>
        /// <param name="updates"></param>
        private async Task<List<HandleUpdateResult>> UpdatesHandle(
            UpdateDto[] updates)
        {
            //TODO: Может уперется в количество соединений в БД при большом количестве обновлений у бота
            var tasks = updates.Select(u => _updateHandleService.HandleUpdate(u));
            var handleResults = await Task.WhenAll(tasks);

            var chatForUpdateIds = handleResults.Select(hr => hr.ChatId).Distinct();

            var handleUpdateResults = new List<HandleUpdateResult>();
            foreach (var chatId in chatForUpdateIds)
            {
                var lastUpdateInChat = handleResults
                                       .Where(hr => hr.ChatId == chatId)
                                       .OrderByDescending(hr => hr.RequestMessageId)
                                       .First();
                handleUpdateResults.Add(lastUpdateInChat);
            }
            return handleUpdateResults;
        }

        private async Task SendUpdateHandleResults(List<HandleUpdateResult> updateHandleResults,
            CancellationToken stoppingToken)
        {
            foreach (var handleUpdateResult in updateHandleResults)
            {
                var sendMessageRequest = new SendMessageRequest
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
