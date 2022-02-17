using Newtonsoft.Json;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет голосовое сообщение.
    /// </summary>
    public class Voice
    {
        /// <summary>
        ///     Уникальный идентификатор файла
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; }

        /// <summary>
        /// 	Продолжительность аудиофайла, заданная отправителем
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 	Опционально. MIME-тип файла, заданный отправителем
        /// </summary>
        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        /// <summary>
        /// 	Опционально. Размер файла
        /// </summary>
        [JsonProperty("file_size")]
        public string FileSize { get; set; }
    }
}