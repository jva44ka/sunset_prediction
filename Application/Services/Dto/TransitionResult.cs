using Domain.Entities.Enums;

namespace Application.Services.Dto;

public class TransitionResult
{
    public AnswerMessageType AnswerMessageType { get; set; }
    public DialogState NewState { get; set; }
    public string? CityAddress { get; set; }
}