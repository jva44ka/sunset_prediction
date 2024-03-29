﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Entities;
using Newtonsoft.Json;

namespace Application.Services;

/// <inheritdoc cref="ICitiesStoreService"/>
public class CitiesStoreService : ICitiesStoreService
{
    private const string CitiesJsonPath = "./SeedData/cities.json";
    //TODO: Сделать слабую ссылку чтобы не держать файл в памяти
    private Location[]? _locations;

    public async Task<City?> FindCityByName(string searchCityName)
    {
        var route = await SearchRouteByCityName(searchCityName);

        if (route == null)
            return null;

        return GenerateCity(route);
    }

    public async Task<City?> FindCityById(int cityId)
    {
        var route = await SearchRouteByCityId(cityId);

        if (route == null)
            return null;

        return GenerateCity(route);
    }

    private City GenerateCity(
        List<Location> route)
    {
        var cityLocation = route.First();
        var countryLocation = route.Last();
        return new City
        {
            Id = cityLocation.Id,
            Address = string.Join(", ", route.Select(location => location.Name)),
            Name = cityLocation.Name,
            NameForUrl = cityLocation.Name.Split('(').First().Trim(),
            CountryCode = countryLocation.CountryCode!
        };
    }

    /// <summary>
    ///     При найденом по названию городе возвращает инвертированную (от города к государству)
    ///     коллекцию локаций, представляющую маршрут по дереву до этой локации
    /// </summary>
    private async Task<List<Location>?> SearchRouteByCityName(string searchLocationName)
    {
        _locations ??= await GetLocationsFromFile();

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
    private async Task<List<Location>?> SearchRouteByCityId(int cityId)
    {
        _locations ??= await GetLocationsFromFile();

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
        var textFromFile = await File.ReadAllTextAsync(CitiesJsonPath);
        return JsonConvert.DeserializeObject<Location[]>(textFromFile)
               ?? throw new SerializationException(
                   "cities.json file deserialization operations returns null");
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
                var route = new List<Location> { this };
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
                var route = new List<Location> { this };
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