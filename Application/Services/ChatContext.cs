﻿using System;
using Application.Services.EntityServices.Interfaces;
using Domain.Entities;
using TelegramApi.Client.Dtos;

namespace Application.Services;

public class ChatContext
{
    public long ChatExternalId { get; set; }
    public UserDto UserDto { get; set; }
    public string? MessageText { get; set; }
    public Chat? ExistingChat { get; set; }
    public User? ExistingUser { get; set; }

    public ICityService CityService { get; }
    public IUserService UserService { get; }
    public IChatService ChatService { get; }

    public ChatContext(
        long chatExternalId,
        UserDto userDto,
        string? messageText,
        Chat? existingChat,
        User? existingUser,
        ICityService cityService,
        IUserService userService,
        IChatService chatService)
    {
        ChatExternalId = chatExternalId;
        UserDto = userDto;
        MessageText = messageText;
        ExistingChat = existingChat;
        ExistingUser = existingUser;

        CityService = cityService;
        UserService = userService;
        ChatService = chatService;
    }

    public void ValidateMessageText()
    {
        if (MessageText == null)
        {
            throw new Exception(
                "Validation of MessageText failed");
        }
    }

    public void ValidateExistingChat()
    {
        if (ExistingChat == null)
        {
            throw new Exception(
                "Validation of ExistingChat failed");
        }
    }

    public void ValidateExistingUser()
    {
        if (ExistingUser == null)
        {
            throw new Exception(
                "Validation of ExistingUser failed");
        }
    }
}