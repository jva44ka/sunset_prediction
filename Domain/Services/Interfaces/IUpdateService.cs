using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IUpdateService
    {
        public Task HandleUpdate(Update update);
    }
}