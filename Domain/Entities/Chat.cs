using Newtonsoft.Json;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет собой чат.
    /// </summary>
    public class Chat
    {
        /// <summary>
        ///     Уникальный идентификатор чата. Абсолютное значение не превышает 1e13
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Тип чата: “private”, “group”, “supergroup” или “channel”
        /// </summary>
        public ChatType Type { get; set; }

        /// <summary>
        ///     Опционально. Название, для каналов или групп
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Опционально. Username, для чатов и некоторых каналов
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Опционально. Имя собеседника в чате
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        ///     Опционально. Фамилия собеседника в чате
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        ///     Опционально. True, если все участники чата являются администраторами
        /// </summary>
        [JsonProperty("all_members_are_administrators")]
        public bool AllMembersAreAdministrators { get; set; }
    }
}