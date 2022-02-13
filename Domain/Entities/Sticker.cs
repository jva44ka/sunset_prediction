using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет стикер.
    /// </summary>
    public class Sticker
    {
        /// <summary>
        ///     Уникальный идентификатор файла
        /// </summary>
        [JsonPropertyName("file_id")]
        public string FileId { get; set; }

        /// <summary>
        /// 	Ширина стикера
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 	Высота стикера
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 	Опционально. Превью стикера в формате .webp или .jpg
        /// </summary>
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// 	Опционально. Размер файла
        /// </summary>
        [JsonPropertyName("file_size")]
        public string FileSize { get; set; }
    }
}