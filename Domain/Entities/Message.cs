using System;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    ///     Этот объект представляет собой сообщение.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 	Уникальный идентификатор сообщения
        /// </summary>
        [JsonPropertyName("message_id")]
        public int MessageId { get; set; }

        /// <summary>
        /// 	Опционально. Отправитель. Может быть пустым в каналах.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        ///     Дата отправки сообщения (Unix time)
        /// </summary>
        public int Date { get; set; }

        /// <summary>
        /// 	Диалог, в котором было отправлено сообщение
        /// </summary>
        public Chat Chat { get; set; }

        /// <summary>
        /// 	Опционально. Для пересланных сообщений: отправитель оригинального сообщения
        /// </summary>
        [JsonPropertyName("forward_from")]
        public User ForwardFrom { get; set; }

        /// <summary>
        /// 	Опционально. Для пересланных сообщений: дата отправки оригинального сообщения
        /// </summary>
        [JsonPropertyName("forward_date")]
        public DateTime ForwardDate { get; set; }

        /// <summary>
        ///     Опционально. Для ответов: оригинальное сообщение.
        ///     Note that the Message object in this field will not contain
        ///     further reply_to_message fields even if it itself is a reply.
        /// </summary>
        [JsonPropertyName("reply_to_message")]
        public Message ReplyToMessage { get; set; }

        /// <summary>
        /// 	Опционально. Для текстовых сообщений: текст сообщения, 0-4096 символов
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     Опционально. Для текстовых сообщений: особые сущности в тексте сообщения.
        /// </summary>
        public MessageEntity[] Entities { get; set; }

        /// <summary>
        /// 	Опционально. Информация об аудиофайле
        /// </summary>
        public Audio Audio { get; set; }

        /// <summary>
        ///     Опционально. Информация о файле
        /// </summary>
        public Document Document { get; set; }

        /// <summary>
        ///     Опционально. Доступные размеры фото
        /// </summary>
        public PhotoSize[] Photo { get; set; }

        /// <summary>
        /// 	Опционально. Информация о стикере
        /// </summary>
        public Sticker Sticker { get; set; }

        /// <summary>
        /// 	Опционально. Информация о видеозаписи
        /// </summary>
        public Video Video { get; set; }

        /// <summary>
        ///     Опционально. Информация о голосовом сообщении
        /// </summary>
        public Voice Voice { get; set; }

        /// <summary>
        ///     Опционально. Подпись к файлу, фото или видео, 0-200 символов
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 	Опционально. Информация об отправленном контакте
        /// </summary>
        public Contact Contact { get; set; }

        /// <summary>
        /// 	Опционально. Информация о местоположении
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// 	Опционально. Информация о месте на карте
        /// </summary>
        public Venue Venue { get; set; }

        /// <summary>
        ///     Опционально. Информация о пользователе, добавленном в группу
        /// </summary>
        [JsonPropertyName("new_chat_member")]
        public User NewChatMember { get; set; }

        /// <summary>
        /// 	Опционально. Информация о пользователе, удалённом из группы
        /// </summary>
        [JsonPropertyName("left_chat_member")]
        public User LeftChatMember { get; set; }

        /// <summary>
        ///     Опционально. Название группы было изменено на это поле
        /// </summary>
        [JsonPropertyName("new_chat_title")]
        public string NewChatTitle { get; set; }

        /// <summary>
        ///     Опционально. Фото группы было изменено на это поле
        /// </summary>
        [JsonPropertyName("new_chat_photo")]
        public PhotoSize[] NewChatPhoto { get; set; }

        /// <summary>
        ///     Опционально. Сервисное сообщение: фото группы было удалено
        /// </summary>
        [JsonPropertyName("delete_chat_photo")]
        public bool DeleteChatPhoto { get; set; }

        /// <summary>
        ///     Опционально. Сервисное сообщение: группа создана
        /// </summary>
        [JsonPropertyName("group_chat_created")]
        public bool GroupChatCreated { get; set; }

        /// <summary>
        ///     Опционально. Сервисное сообщение: супергруппа создана
        /// </summary>
        [JsonPropertyName("supergroup_chat_created")]
        public bool SupergroupChatCreated { get; set; }

        /// <summary>
        ///     Опционально. Сервисное сообщение: канал создан
        /// </summary>
        [JsonPropertyName("channel_chat_created")]
        public bool ChannelChatCreated { get; set; }

        /// <summary>
        ///     Опционально. Группа была преобразована в супергруппу с указанным идентификатором. Не превышает 1e13
        /// </summary>
        [JsonPropertyName("migrate_to_chat_id")]
        public int MigrateToChatId { get; set; }

        /// <summary>
        ///     Опционально. Cупергруппа была создана из группы с указанным идентификатором. Не превышает 1e13
        /// </summary>
        [JsonPropertyName("migrate_from_chat_id")]
        public int MigrateFromChatId { get; set; }

        /// <summary>
        ///     Опционально. Указанное сообщение было прикреплено.
        ///     Note that the Message object in this field will not contain further reply_to_message
        ///     fields even if it is itself a reply.
        /// </summary>
        [JsonPropertyName("pinned_message")]
        public Message PinnedMessage { get; set; }
    }
}