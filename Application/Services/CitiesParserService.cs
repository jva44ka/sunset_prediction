using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Entities;
using Newtonsoft.Json;

namespace Application.Services
{
    /// <inheritdoc cref="ICitiesParserService"/>
    public class CitiesParserService : ICitiesParserService
    {
        private const string CitiesJsonPath = "./SeedData/cities.json";
        //TODO: Сделать слабую ссылку чтобы не держать файл в памяти
        private Location[]? _locations;

        public async Task<City?> FindCity(string searchCityName)
        {
            var route = await SearchRoute(searchCityName).ConfigureAwait(false);

            if (route != null)
            {
                var cityLocation = route.First();
                var countryLocation = route.Last();
                return new City
                {
                    Id = cityLocation.Id,
                    Address = string.Join(", ", route.Select(location => location.Name)),
                    Name = cityLocation.Name,
                    UrlName = cityLocation.Name.Split('(').First().Trim(),
                    CountryCode = countryLocation.CountryCode
                };
            }

            return null;
        }
        
        public async Task<City?> FindCity(int cityId)
        {
            var route = await SearchRoute(cityId).ConfigureAwait(false);

            if (route != null)
            {
                var cityLocation = route.First();
                var countryLocation = route.Last();
                return new City
                {
                    Id = cityLocation.Id,
                    Address = string.Join(", ", route.Select(location => location.Name)),
                    Name = cityLocation.Name,
                    UrlName = cityLocation.Name.Split('(').First().Trim(),
                    CountryCode = countryLocation.CountryCode
                };
            }

            return null;
        }

        /// <summary>
        ///     При найденом по названию городе возвращает инвертированную (от города к государству)
        ///     коллекцию локаций, представляющую маршрут по дереву до этой локации
        /// </summary>
        private async Task<List<Location>?> SearchRoute(string searchLocationName)
        {
            _locations ??= await GetLocationsFromFile().ConfigureAwait(false);

            foreach (var location in _locations)
            {
                var matchLocationRoute = location.SearchRoute(searchLocationName);

                if (matchLocationRoute != null)
                {
                    return matchLocationRoute;
                }
            }

            return null;
        }
        
        /// <summary>
        ///     При найденом по названию городе возвращает инвертированную (от города к государству)
        ///     коллекцию локаций, представляющую маршрут по дереву до этой локации
        /// </summary>
        private async Task<List<Location>?> SearchRoute(int cityId)
        {
            _locations ??= await GetLocationsFromFile().ConfigureAwait(false);

            foreach (var location in _locations)
            {
                var matchLocationRoute = location.SearchRoute(cityId);

                if (matchLocationRoute != null)
                {
                    return matchLocationRoute;
                }
            }

            return null;
        }

        private async Task<Location[]> GetLocationsFromFile()
        {
            var textFromFile = await File.ReadAllTextAsync(CitiesJsonPath)
                                         .ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Location[]>(textFromFile);
        }

        /// <summary>
        ///     Нода локации при десериализации файла SeedData/cities.json
        /// </summary>
        class Location
        {
            public int Id { get; set; }

            [JsonProperty("parent_id")]
            public int? ParentId { get; set; }

            [JsonProperty("country_code")]
            public string? CountryCode { get; set; }
            public string Name { get; set; }
            public Location[] Areas { get; set; }
            
            /// <summary>
            ///     При найденом по названию городе возвращает инвертированную (от города к государству)
            ///     коллекцию локаций, представляющую маршрут по дереву до этой локации
            /// </summary>
            public List<Location>? SearchRoute(string searchName)
            {
                if (Name.ToLower().Contains(searchName.ToLower()) && Areas.Length == 0)
                {
                    var route = new List<Location> {this};
                    return route;
                }

                foreach (var area in Areas)
                {
                    var route = area.SearchRoute(searchName);

                    if (route != null)
                    {
                        route.Add(this);
                        return route;
                    }
                }

                return null;
            }
            
            /// <summary>
            ///     При найденом по названию городе возвращает инвертированную (от города к государству)
            ///     коллекцию локаций, представляющую маршрут по дереву до этой локации
            /// </summary>
            public List<Location>? SearchRoute(int cityId)
            {
                if (Id.Equals(cityId))
                {
                    var route = new List<Location> {this};
                    return route;
                }

                foreach (var area in Areas)
                {
                    var route = area.SearchRoute(cityId);

                    if (route != null)
                    {
                        route.Add(this);
                        return route;
                    }
                }

                return null;
            }
        }
    }
}