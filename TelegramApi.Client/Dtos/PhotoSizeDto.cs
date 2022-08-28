using Newtonsoft.Json;

namespace TelegramApi.Client.Dtos;

/// <summary>
///     Этот объект представляет изображение определённого размера или превью файла / стикера.
/// </summary>
public class PhotoSizeDto
{
    /// <summary>
    /// 	Уникальный идентификатор файла
    /// </summary>
    [JsonProperty("file_id")]
    public string FileId { get; set; } = default!;

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
    [JsonProperty("file_size")]
    public int? FileSize { get; set; }
}