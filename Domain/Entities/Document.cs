using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет файл, не являющийся фотографией, голосовым сообщением или аудиозаписью.
    /// </summary>
    public class Document
    {
        /// <summary>
        ///     Unique file identifier
        /// </summary>
        [JsonPropertyName("file_id")]
        public string FileId { get; set; }

        /// <summary>
        ///     Опционально. Document thumbnail as defined by sender
        /// </summary>
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// 	Опционально. Original filename as defined by sender
        /// </summary>
        [JsonPropertyName("file_name")]
        public string FileName { get; set; }

        /// <summary>
        /// 	Опционально. MIME файла, заданный отправителем
        /// </summary>
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }
        
        /// <summary>
        /// 	Опционально. Размер файла
        /// </summary>
        [JsonPropertyName("file_size")]
        public string FileSize { get; set; }
    }
}