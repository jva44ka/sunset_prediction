namespace Domain.Entities
{
    public class City
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
    }
}