using System;
using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class User
    {
        /// <summary>
        ///     Идентификатор пользователя в Telegram
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Идентификатор города, который выбрал пользователь
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        ///     Имя пользователя в Telegram
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        ///     Фамилия пользователя в Telegram
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        ///     Никнейм пользователя в Telegram
        /// </summary>
        public string UserName { get; set; } = default!;

        /// <summary>
        ///     Предыдущее состояние диалога бота с пользователем
        /// </summary>
        public DialogState? PreviousDialogState { get; set; }

        /// <summary>
        ///     Текущее состояние диалога бота с пользователем
        /// </summary>
        public DialogState CurrentDialogState { get; set; }

        /// <summary>
        ///     Дата последнего изменения состояния диалога
        /// </summary>
        public DateTime StateChangeDate { get; set; }
    }
}
