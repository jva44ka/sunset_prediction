﻿using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет файл, готовый к загрузке. Он может быть скачан по ссылке вида
    ///     https://api.telegram.org/file/bot<token>/<file_path>. Ссылка будет действительна как минимум
    ///     в течение 1 часа. По истечении этого срока она может быть запрошена заново с помощью метода getFile.
    /// </summary>
    public class File
    {
        /// <summary>
        ///     Уникальный идентификатор файла
        /// </summary>
        [JsonPropertyName("file_id")]
        public string FileId { get; set; }

        /// <summary>
        ///     Опционально. Размер файла, если известен
        /// </summary>
        [JsonPropertyName("file_size")]
        public int FileSize { get; set; }

        /// <summary>
        ///     Опционально. Расположение файла. Для скачивания воспользуйтейсь ссылкой вида
        ///     https://api.telegram.org/file/bot<token>/<file_path>
        /// </summary>
        [JsonPropertyName("file_path")]
        public string FilePath { get; set; }
    }
}