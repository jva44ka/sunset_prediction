using Application.Services.EntityServices.Interfaces;
using TelegramApi.Client.Dtos;

namespace Application.Services;

public class ChatContext
{
    public long ChatExternalId { get; set; }
    public UserDto UserDto { get; set; }
    public string MessageText { get; set; }
    public ICityService CityService { get; }
    public IUserService UserService { get; }
    public IChatService ChatService { get; }

    public ChatContext(
        MessageDto message, 
        ICityService cityService, 
        IUserService userService, 
        IChatService chatService)
    {
        CityService = cityService;
        UserService = userService;
        ChatService = chatService;
        var chatExternalId = message.Chat.Id;
        var userDto = message.From;
        var messageText = message.Text;
    }
}