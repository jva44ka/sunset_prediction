namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет одну из особых сущностей в текстовом сообщении.
    ///     Например: хештеги, имена пользователей, ссылки итд.
    /// </summary>
    public class MessageEntity
    {
        /// <summary>
        ///     Type of the entity. One of mention (@username), hashtag, bot_command, url, email, bold (bold text),
        ///     italic (italic text), code (monowidth string), pre (monowidth block), text_link (for clickable text URLs)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Offset in UTF-16 code units to the start of the entity
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        ///     Length of the entity in UTF-16 code units
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        ///     Опционально. For “text_link” only, url that will be opened after user taps on the text
        /// </summary>
        public string Url { get; set; }
    }
}