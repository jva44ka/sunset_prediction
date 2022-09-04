using System.Collections.Generic;
using System.Linq;
using Application.Services.Interfaces;
using Domain.Entities.Enums;
using TelegramApi.Client.Dtos;

namespace Application.Services;

public class AnswerService : IAnswerService
{
    private readonly Dictionary<AnswerMessageType, string> _messageTexts 
        = new Dictionary<AnswerMessageType, string>
    {
        {
            AnswerMessageType.ProposedInputCity,
            "Пожалуйста введите название своего города (желательно точное название)."
        },
        {
            AnswerMessageType.ProposedFoundedCityName,
            "Ваш город {0}?"
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

    private readonly Dictionary<AnswerMessageType, ReplyKeyboardMarkupDto> _messageKeyboards 
        = new Dictionary<AnswerMessageType, ReplyKeyboardMarkupDto>
    {
        {
            AnswerMessageType.ProposedFoundedCityName, 
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Да", "Нет")
        },
        {
            AnswerMessageType.ProposedSubscribeTypes, 
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Обычная", "Двойная")
        },
        {
            AnswerMessageType.SubscribedToEverydayPushes,
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Отписка")
        },
        {
            AnswerMessageType.SubscribedToEverydayDoublePushes,
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Отписка")
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

    public string GenerateAnswerText(AnswerMessageType messageType, params string[] args)
    {
        var messageText = _messageTexts[messageType];
        
        args.Where(arg => string.IsNullOrEmpty(arg) == false)
            .Select((arg, index) => messageText = messageText.Replace($"{{{index}}}", arg))
            .ToArray();

        return messageText;
    }

    public ReplyKeyboardMarkupDto? GenerateKeyboard(AnswerMessageType messageType)
    {
        try
        {
            return _messageKeyboards[messageType];
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }
}