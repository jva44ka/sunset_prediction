using System;
using Domain.Entities.Enums;

namespace Application.Services.Dto;

public class AnswerDto
{
    public AnswerMessageType MessageType { get; set; }
    public string[] MessageArgs { get; set; } = Array.Empty<string>();
}