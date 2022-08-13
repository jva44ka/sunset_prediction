namespace Domain.Entities
{
    public class City
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Название города + название области при наличии нескольких городов с таким названием
        /// </summary>
        /// <example>Санкт-Петербург</example>
        /// <example>Михайловка (Иркутская область)</example>
        public string Name { get; set; } = default!;

        /// <summary>
        ///     Название города без уточнения областей и пробелов
        /// </summary>
        /// <example>
        ///     "Михайловка" вместо "Михайловка (Иркутская область)"
        /// </example>
        public string NameForUrl { get; set; } = default!;

        /// <summary>
        ///     Полный адрес населенного пункста, включая область и страну
        /// </summary>
        /// <example>
        ///     Красногорский (Республика Марий Эл), Республика Марий Эл, Россия
        /// </example>
        public string Address { get; set; } = default!;

        /// <summary>
        ///     Код страны
        /// </summary>
        /// <example>RU</example>
        public string CountryCode { get; set; } = default!;

        /// <summary>
        ///     Широта
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        ///     Долгота
        /// </summary>
        public double Longitude { get; set; }
    }
}