﻿using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет аудиозапись, которую клиенты Telegram воспинимают как музыкальный трек.
    /// </summary>
    public class Audio
    {
        /// <summary>
        ///     Уникальный идентификатор файла
        /// </summary>
        [JsonPropertyName("file_id")]
        public string FileId { get; set; }

        /// <summary>
        ///     Duration of the audio in seconds as defined by sender
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        ///     Опционально. Performer of the audio as defined by sender or by audio tags
        /// </summary>
        public string Performer { get; set; }

        /// <summary>
        ///     Опционально. Title of the audio as defined by sender or by audio tags
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Опционально. MIME файла, заданный отправителем
        /// </summary>
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }
        
        /// <summary>
        ///     Опционально. MIME файла, заданный отправителем
        /// </summary>
        [JsonPropertyName("file_size")]
        public string FileSize { get; set; }
    }
}