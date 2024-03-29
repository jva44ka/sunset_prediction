﻿using System;
using System.Threading.Tasks;
using Dapper;
using DataAccess.ConnectionFactories;
using DataAccess.Dao.Interfaces;
using Domain.Entities;
using Domain.Entities.Enums;

namespace DataAccess.Dao;

public class ChatDao : IChatDao
{
    private readonly IConnectionFactory _connectionFactory;

    public ChatDao(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Chat?> GetByExternalId(long externalId)
    {
        string sql =
            @"
SELECT
    c.id                AS  Id,
    c.external_id       AS  ExternalId,
    c.previous_state    AS  PreviousState,
    c.current_state     AS  CurrentState,
    c.state_changed_at  AS  StateChangedAt
FROM 
    chats c
WHERE
    c.external_id = @ExternalId";
        using var connection = await _connectionFactory.CreateConnection();
        var chat = await connection.QueryFirstOrDefaultAsync<Chat>(sql, new
        {
            ExternalId = externalId
        });

        return chat;
    }
        
    public async Task<bool> Create(Chat chat)
    {
        string sql =
            @"
INSERT INTO chats (
    external_id,
    previous_state,
    current_state,
    state_changed_at
)
VALUES (
    @ExternalId,
    @PreviousState,
    @CurrentState,
    @StateChangedAt
)";
        using var connection = await _connectionFactory.CreateConnection();
        var rowsInserted = await connection.ExecuteAsync(sql, new
        {
            chat.ExternalId,
            chat.PreviousState,
            CurrentState = chat.CurrentState,
            chat.StateChangedAt,
        });
        return rowsInserted == 1;
    }

    public async Task<bool> UpdateState(
        int chatId,
        ChatStateType previousState,
        ChatStateType currentState,
        DateTime stateChangeDate)
    {
        string sql =
            @"
UPDATE
    chats
SET
    previous_state = @PreviousState,
    current_state = @CurrentState,
    state_changed_at = @StateChangedAt
WHERE
    id = @ChatId";
        using var connection = await _connectionFactory.CreateConnection();
        var rowsUpdated = await connection.ExecuteAsync(sql, new
        {
            ChatId = chatId,
            PreviousState = previousState,
            CurrentState = currentState,
            StateChangedAt = stateChangeDate
        });
        return rowsUpdated == 1;
    }
}