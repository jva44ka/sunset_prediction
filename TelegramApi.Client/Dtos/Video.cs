using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos
{
    /// <summary>
    ///     Этот объект представляет видеозапись.
    /// </summary>
    public class Video
    {
        /// <summary>
        ///     Уникальный идентификатор файла
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; } = default!;

        /// <summary>
        /// 	Ширина видео, заданная отправителем
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 	Высота видео, заданная отправителем
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 	Продолжительность видео, заданная отправителем
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 	Опционально. Превью стикера в формате .webp или .jpg
        /// </summary>
        public PhotoSize? Thumb { get; set; }

        /// <summary>
        /// 	Опционально. MIME файла, заданный отправителем
        /// </summary>
        [JsonProperty("mime_type")]
        public string? MimeType { get; set; }

        /// <summary>
        /// 	Опционально. Размер файла
        /// </summary>
        [JsonProperty("file_size")]
        public string? FileSize { get; set; }
    }
}