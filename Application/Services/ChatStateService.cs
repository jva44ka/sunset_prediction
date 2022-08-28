using System.Threading.Tasks;
using Application.Services.Dto;
using Application.Services.EntityServices.Interfaces;
using Application.Services.Interfaces;
using TelegramApi.Client.Dtos;

namespace Application.Services;

public class ChatStateService : IChatStateService
{
    private readonly IUserService _userService;
    private readonly IChatService _chatService;
    private readonly ICityService _cityService;
    private readonly IChatStateFactory _chatStateFactory;

    public ChatStateService(
        IUserService userService,
        IChatService chatService,
        ICityService cityService,
        IChatStateFactory chatStateFactory)
    {
        _userService = userService;
        _chatService = chatService;
        _cityService = cityService;
        _chatStateFactory = chatStateFactory;
    }

    public async Task<AnswerDto> Transit(
        MessageDto messageDto)
    {
        var chatExternalId = messageDto.Chat.Id;
        var userDto = messageDto.From;
        var messageText = messageDto.Text;

        var existingChat = await _chatService.GetByExternalId(chatExternalId);
        var existingUser = await _userService.GetByExternalId(userDto.Id);

        var chatContext = new ChatContext(
            chatExternalId,
            userDto,
            messageText,
            existingChat,
            existingUser,
            _cityService,
            _userService,
            _chatService);

        var state = _chatStateFactory.Create(existingChat?.CurrentState, chatContext);
        return await state.HandleTextMessage();
    }
}