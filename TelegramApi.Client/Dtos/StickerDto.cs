using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos
{
    /// <summary>
    ///     Этот объект представляет стикер.
    /// </summary>
    public class StickerDto
    {
        /// <summary>
        ///     Уникальный идентификатор файла
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; } = default!;

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
        public PhotoSizeDto? Thumb { get; set; }

        /// <summary>
        /// 	Опционально. Размер файла
        /// </summary>
        [JsonProperty("file_size")]
        public string? FileSize { get; set; }
    }
}