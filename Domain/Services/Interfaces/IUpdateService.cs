﻿using System.Threading.Tasks;
using Domain.Entities.TelegramApi;

namespace Domain.Services.Interfaces
{
    public interface IUpdateService
    {
        public Task<int?> GetLastUpdateId();

        /// <summary>
        ///     Обрабатывает <see cref="Update"/>
        /// </summary>
        /// <returns>Сообщение, которое нужно отправить пользователю в ответ</returns>
        public Task<string> HandleUpdate(Update update);
    }
}