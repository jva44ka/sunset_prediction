using System;
using Domain.Entities.Enums;

namespace Application.Services.Dto;

public class TransitionResult
{
    public AnswerMessageType AnswerMessageType { get; set; }
    public ChatStateType NewState { get; set; }
    public string[] AnswerMessageArgs { get; set; } = Array.Empty<string>();
}