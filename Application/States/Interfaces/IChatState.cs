using System.Threading.Tasks;
using Application.Services.Dto;

namespace Application.States.Interfaces;

public interface IChatState
{
    public Task<AnswerDto> HandleTextMessage();
}