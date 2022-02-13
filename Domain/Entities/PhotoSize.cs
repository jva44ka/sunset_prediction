using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет изображение определённого размера или превью файла / стикера.
    /// </summary>
    public class PhotoSize
    {
        /// <summary>
        /// 	Уникальный идентификатор файла
        /// </summary>
        [JsonPropertyName("file_id")]
        public string FileId { get; set; }

        /// <summary>
        /// 	Photo width
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        ///     Photo height
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        ///     Опционально. Размер файла
        /// </summary>
        [JsonPropertyName("file_size")]
        public int FileSize { get; set; }
    }
}