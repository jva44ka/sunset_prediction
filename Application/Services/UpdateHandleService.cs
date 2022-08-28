using System;
using System.Threading.Tasks;
using Application.Mappers.Interfaces;
using Application.Services.Dto;
using Application.Services.Interfaces;
using DataAccess.Dao.Interfaces;
using TelegramApi.Client.Dtos;

namespace Application.Services;

public class UpdateHandleService : IUpdateHandleService
{
    private readonly IUpdateDao _updateDao;
    private readonly IChatStateService _chatStateService;
    private readonly IAnswerService _answerService;
    private readonly IMapper<Domain.Entities.Update, UpdateDto> _updatesMapper;

    public UpdateHandleService(
        IUpdateDao updateDao,
        IChatStateService chatStateService,
        IAnswerService answerService,
        IMapper<Domain.Entities.Update, UpdateDto> updatesMapper)
    {
        _updateDao = updateDao;
        _chatStateService = chatStateService;
        _answerService = answerService;
        _updatesMapper = updatesMapper;
    }

    public async Task<long?> GetLastUpdateExternalId()
    {
        var lastUpdate = await _updateDao.GetLastUpdate();
        return lastUpdate?.ExternalId;
    }

    public async Task<HandleUpdateResult> HandleUpdate(UpdateDto updateDto)
    {
        var update = _updatesMapper.ToEntity(updateDto)!;
        update.HandledAt = DateTime.UtcNow;
        var creationResult = await _updateDao.Create(update);

        if (creationResult == false)
        {
            throw new Exception(
                $"Failed to create record in table \"updates\" with update_id: {updateDto.UpdateId}");
        }

        var transitionResult = await _chatStateService.Transit(
            updateDto.Message);

        var messageText = _answerService.GenerateAnswerText(
            transitionResult.MessageType,
            transitionResult.MessageArgs);
        var keyboard = _answerService.GenerateKeyboard(
            transitionResult.MessageType);

        return new HandleUpdateResult
        {
            ChatId = updateDto.Message.Chat.Id,
            MessageText = messageText,
            MessageKeyboard = keyboard
        };
    }
}