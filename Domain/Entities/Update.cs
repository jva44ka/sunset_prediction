using System;

namespace Domain.Entities
{
    public class Update
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        public int UpdateId { get; set; }

        /// <summary>
        ///     Дата обработки обновления ботом
        /// </summary>
        public DateTime HandleDate { get; set; }
    }
}
