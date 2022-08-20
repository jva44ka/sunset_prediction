using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos
{
    /// <summary>
    ///     Этот объект представляет файл, не являющийся фотографией, голосовым сообщением или аудиозаписью.
    /// </summary>
    public class DocumentDto
    {
        /// <summary>
        ///     Unique file identifier
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; } = default!;

        /// <summary>
        ///     Опционально. DocumentDto thumbnail as defined by sender
        /// </summary>
        public PhotoSizeDto? Thumb { get; set; }

        /// <summary>
        /// 	Опционально. Original filename as defined by sender
        /// </summary>
        [JsonProperty("file_name")]
        public string? FileName { get; set; }

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