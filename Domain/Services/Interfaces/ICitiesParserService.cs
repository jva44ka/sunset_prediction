using System.Threading.Tasks;

namespace Domain.Services.Interfaces
{
    /// <summary>
    ///     Сервис работы с данными о городах из SeedData/cities.json
    /// </summary>
    public interface ICitiesParserService
    {
        /// <summary>
        ///     При совпадении названия города возвращает полный адресс ( например "страна, субъект, город")
        ///     Иначе возвращает null
        /// </summary>
        Task<string?> FindFullAddress(string searchCityName);
    }
}