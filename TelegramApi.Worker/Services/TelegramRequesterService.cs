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
using TelegramApi.Worker.Services.Interfaces;

namespace TelegramApi.Worker.Services;

public class TelegramRequesterService : ITelegramRequesterService
{
    private readonly IUpdateHandleService _updateHandleService;
    private readonly ITelegramBotApiClient _telegramBotApiClient;
    private readonly ILogger<TelegramRequesterService> _logger;

    public TelegramRequesterService(
        IUpdateHandleService updateHandleService,
        ITelegramBotApiClient telegramBotApiClient,
        ILogger<TelegramRequesterService> logger)
    {
        _updateHandleService = updateHandleService;
        _telegramBotApiClient = telegramBotApiClient;
        _logger = logger;
    }
        
    public async Task HandleNewUpdates(CancellationToken cancellationToken)
    {
        var lastHandledUpdateId = await _updateHandleService.GetLastUpdateExternalId();
        var response = await _telegramBotApiClient.GetUpdates(lastHandledUpdateId, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var responseContentString = await response.Content.ReadAsStringAsync(cancellationToken);
            var newUpdatesResponse = JsonConvert.DeserializeObject<GetUpdatesResult>(responseContentString);

            if (newUpdatesResponse.Ok && newUpdatesResponse.Result.Length > 0)
            {
                var updateHandleResults = await HandleUpdates(newUpdatesResponse.Result);
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
    private async Task<HandleUpdateResult[]> HandleUpdates(
        UpdateDto[] updates)
    {
        //TODO: Может уперется в количество соединений в БД при большом количестве обновлений у бота
        var chatForUpdateIds = updates.Select(u => u.Message.Chat.Id).Distinct();
        var updatesToHandle = new List<UpdateDto>();
        foreach (var chatId in chatForUpdateIds)
        {
            var lastUpdateInChat = updates
                .Where(u => u.Message.Chat.Id == chatId)
                .OrderByDescending(u => u.Message.MessageId)
                .First();
            updatesToHandle.Add(lastUpdateInChat);
        }

        var tasks = updatesToHandle.Select(u => _updateHandleService.HandleUpdate(u));
        var handleResults = await Task.WhenAll(tasks);
        return handleResults.ToArray();
    }

    private async Task SendUpdateHandleResults(
        HandleUpdateResult[] updateHandleResults,
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