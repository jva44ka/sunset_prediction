using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos
{
    /// <summary>
    ///     Этот объект представляет голосовое сообщение.
    /// </summary>
    public class VoiceDto
    {
        /// <summary>
        ///     Уникальный идентификатор файла
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; } = default!;

        /// <summary>
        /// 	Продолжительность аудиофайла, заданная отправителем
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 	Опционально. MIME-тип файла, заданный отправителем
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