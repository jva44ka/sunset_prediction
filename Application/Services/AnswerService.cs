using System;
using System.Collections.Generic;
using Domain.Entities.Enums;
using TelegramApi.Client.Dtos;

namespace Application.Services;

public class AnswerService : IAnswerService
{
    private Dictionary<AnswerMessageType, string> _messageTexts 
        = new Dictionary<AnswerMessageType, string>
    {
        {
            AnswerMessageType.InputCity,
            "Пожалуйста введите название своего города (желательно точное название)."
        },
        {
            AnswerMessageType.ProposedCityName,
            "Ваш город {cityAddress}?"
        },
        {
            AnswerMessageType.CityNameNotFound,
            "Город с таким названием не найден. Попробуйте написать точное название вашего города."
        },
        {
            AnswerMessageType.ProposedSubscribeTypes,
            "Выбирите и напишите вариант рассылки:\n" +
                "1. 'Обычная' - бот присылает одно сообщение за час до заката при высокой вероятности заката \n" +
                "2. 'Двойная' - бот присылает одно сообщение с утра, второе сообщение за час до заката при высокой вероятности заката"
        },
        {
            AnswerMessageType.ProposedCityNameWrong,
            "Возможно вы ввели неполное название города, попробуйте еще раз."
        },
        {
            AnswerMessageType.SubscribedToEverydayPushes,
            "Вы подписались на обычную рассылку. В случае успешного прогноза " +
                "заката вам будет отправлено уведомление за час.\n" +
                "Если вы хотите отписаться от рассылки напишите 'Отписка'."
        },
        {
            AnswerMessageType.SubscribedToEverydayDoublePushes,
            "Вы подписались на двойную рассылку. В случае успешного прогноза заката вам будет " +
                "отправлено уведомление утром. Если вечером того же дня прогноз будет все еще успешен, " +
                "вам отправится второе уведомление за час до заката. \n" +
                "Если вы хотите отписаться от рассылки напишите 'Отписка'."
        },
        {
            AnswerMessageType.InputSubscribeNameWrong,
            "Введеный вариант подписки не распознан. Пожалуйста напишите один " +
                "из вариантов: 'Обычная' или 'Двойная'."
        },
        {
            AnswerMessageType.UnsubscribeWarning,
            "Вы действительно хотите отписаться от рассылки?"
        },
        {
            AnswerMessageType.Unsubscribed,
            "Вы отписаны от всех рассылок. \n" +
                "Если вы хотите опять подписаться на рассылку прогноза - от рассылки напишите 'Подписка'"
        },
        {
            AnswerMessageType.StaysSubscribed,
            "Вы подписаны на рассылку. \n" +
                "Если вы хотите отписаться от рассылки напишите 'Отписка'."
        },
        {
            AnswerMessageType.StaysUnsubscribed,
            "Вы отписаны от всех рассылок. \n" +
                "Если вы хотите опять подписаться на рассылку прогноза - от рассылки напишите 'Подписка'"
        }
    };

    private Dictionary<AnswerMessageType, ReplyKeyboardMarkupDto> _messageKeyboards 
        = new Dictionary<AnswerMessageType, ReplyKeyboardMarkupDto>
    {
        {
            AnswerMessageType.ProposedCityName, 
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Да", "Нет")
        },
        {
            AnswerMessageType.ProposedSubscribeTypes, 
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Обычная", "Двойная")
        },
        {
            AnswerMessageType.UnsubscribeWarning, 
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Да", "Нет")
        },
        {
            AnswerMessageType.Unsubscribed, 
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Подписка")
        }
    };

    public AnswerService()
    { }

    public string GenerateAnswerText(AnswerMessageType messageType)
    {
        var messageText = _messageTexts[messageType];
        return messageText
               ?? throw new ArgumentOutOfRangeException(
                   nameof(messageType));
    }

    public ReplyKeyboardMarkupDto? GenerateKeyboard(AnswerMessageType messageType)
    {
        return _messageKeyboards[messageType];
    }
}