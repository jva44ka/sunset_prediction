﻿using System.Threading.Tasks;
using Domain.Entities;

namespace DataAccess.Dao.Interfaces;

/// <summary>
///     DAO для работы с таблицей users
/// </summary>
public interface IUpdateDao
{
    /// <summary>
    ///     Создает запись в бд об обновлении у бота телеграма
    /// </summary>
    /// <returns>Ture, если корректно создалась запись в БД</returns>
    Task<bool> Create(Update update);

    /// <summary>
    ///     Возвращает последний апдейт бота телеграма
    /// </summary>
    Task<Update?> GetLastUpdate();
}