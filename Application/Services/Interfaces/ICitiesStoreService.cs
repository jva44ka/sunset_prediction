using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    /// <summary>
    ///     Сервис работы с данными о городах из SeedData/cities.json
    /// </summary>
    public interface ICitiesStoreService
    {
        /// <summary>
        ///     Получение города по названию.
        /// </summary>
        /// <remarks>При совпадении названия города возвращает объект <see cref="City"/></remarks>
        Task<City?> FindCity(string searchCityName);

        /// <summary>
        ///     Получение города по id
        /// </summary>
        /// <remarks>
        ///     При совпадении id города возвращает объект <see cref="City"/>.
        ///     Иначе возвращает null
        /// </remarks>
        Task<City?> FindCity(int id);
    }
}