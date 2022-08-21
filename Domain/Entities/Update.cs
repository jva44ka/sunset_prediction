using System;

namespace Domain.Entities
{
    public class Update
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        ///     Идентификатор
        /// </summary>
        public long ExternalId { get; set; }

        /// <summary>
        ///     Дата обработки обновления ботом
        /// </summary>
        public DateTime HandleDate { get; set; }
    }
}
