using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IUpdateService
    {
        public Task<int?> GetLastUpdateId();
        public Task HandleUpdate(Update update);
    }
}