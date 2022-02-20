namespace DataAccess.DAL
{
    public class CityDal
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        ///     Только имя города без уточнения областей и пробелов
        /// </summary>
        /// <example>
        ///     Например "Благовещенск" вместо "Благовещенск (Амурская область)"
        /// </example>
        public string UrlName { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}