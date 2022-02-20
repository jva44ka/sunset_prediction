using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Services.Interfaces
{
    /// <summary>
    ///     Сервис работы с данными о городах из SeedData/cities.json
    /// </summary>
    public interface ICitiesParserService
    {
        /// <summary>
        ///     При совпадении названия города возвращает объект <see cref="City"/>.
        ///     Иначе возвращает null
        /// </summary>
        Task<City?> FindCity(string searchCityName);
        
        /// <summary>
        ///     При совпадении id города возвращает объект <see cref="City"/>.
        ///     Иначе возвращает null
        /// </summary>
        Task<City?> FindCity(int id);
    }
}