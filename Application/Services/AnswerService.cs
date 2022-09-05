using System.Collections.Generic;
using System.Linq;
using Application.Services.Interfaces;
using Domain.Entities.Enums;
using TelegramApi.Client.Dtos;
using Application.Enums;

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
            AnswerMessageType.ProposedCityNameWrong,
            "Возможно вы ввели неполное название города, попробуйте еще раз."
        },
        {
            AnswerMessageType.RequestedNewSubscribeWithoutSubscribes,
            "Чтобы подписаться на рассылку введите название рассылки.\n" +
            "Вам доступны рассылки: 'Грозы', 'Закаты', 'Закаты и грозы'."
        },
        {
            AnswerMessageType.RequestedNewSubscribeWithSunsetSubscribe,
            "Чтобы подписаться на рассылку введите название рассылки.\n" +
            "Вам доступна рассылка 'Грозы'."
        },
        {
            AnswerMessageType.RequestedNewSubscribeWithLightningSubscribed,
            "Чтобы подписаться на рассылку введите название рассылки.\n" +
            "Вам доступна рассылка: 'Закаты'."
        },

        {
            AnswerMessageType.RequestedUnsubscribeWithSunsetAndLightningSubscribes,
            "Чтобы отписаться от рассылки введите название рассылки.\n" +
            "Вы подписаны на доступны 'Грозы', 'Закаты', 'Закаты и грозы'."

        },
        {
            AnswerMessageType.RequestedUnsubscribeWithSunsetSubscribed,
            "Чтобы отписаться от рассылки введите название рассылки.\n" +
            "Вы подписаны на доступны 'Закаты'."
        },
        {
            AnswerMessageType.RequestedUnsubscribeWithLightningSubscribe,
            "Чтобы отписаться от рассылки введите название рассылки.\n" +
            "Вы подписаны на доступны 'Грозы'."
        },
        {
            AnswerMessageType.InputSubscribeNameWrong,
            "Введеный вариант подписки не распознан. Пожалуйста напишите корректное название подписки."
        },
        {
            AnswerMessageType.Unsubscribed,
            "Вы отписаны от всех рассылок. \n" +
                "Если вы хотите подписаться на рассылку прогноза - от рассылки напишите 'Подписка'"
        },
        {
            AnswerMessageType.SubscribedToSunset,
            "Вы подписаны на рассылки закатов. \n" +
            "Если вы хотите подписаться на еще одну рассылку напишите 'Подписка'.\n" +
            "Если вы хотите отписаться от рассылки напишите 'Отписка'.\n"
        },
        {
            AnswerMessageType.SubscribedToLightning,
            "Вы подписаны на рассылки гроз. \n" +
            "Если вы хотите подписаться на еще одну рассылку напишите 'Подписка'.\n" +
            "Если вы хотите отписаться от рассылки напишите 'Отписка'.\n"
        },
        {
            AnswerMessageType.SubscribedToSunsetAndLightning,
            "Вы подписаны на рассылки закатов и гроз. \n" +
            "Если вы хотите отписаться от рассылки напишите 'Отписка'."
        },
    };

    private readonly Dictionary<AnswerMessageType, ReplyKeyboardMarkupDto> _messageKeyboards 
        = new Dictionary<AnswerMessageType, ReplyKeyboardMarkupDto>
    {
        {
            AnswerMessageType.ProposedFoundedCityName, 
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Да", "Нет")
        },
        {
            AnswerMessageType.RequestedNewSubscribeWithoutSubscribes, 
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Грозы", "Закаты", "Закаты и грозы")
        },
        {
            AnswerMessageType.RequestedNewSubscribeWithSunsetSubscribe, 
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Грозы")
        }, 
        {
            AnswerMessageType.RequestedNewSubscribeWithLightningSubscribed, 
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Закаты")
        }, 
        {
            AnswerMessageType.RequestedUnsubscribeWithSunsetAndLightningSubscribes, 
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Грозы", "Закаты", "От всех")
        },
        {
            AnswerMessageType.RequestedUnsubscribeWithLightningSubscribe,
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Грозы")
        },
        {
            AnswerMessageType.RequestedUnsubscribeWithSunsetSubscribed,
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Закаты")
        },
        {
            AnswerMessageType.SubscribedToSunset,
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Подписка", "Отписка")
        },
        {
            AnswerMessageType.SubscribedToLightning,
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Подписка", "Отписка")
        },
        {
            AnswerMessageType.SubscribedToSunsetAndLightning,
            ReplyKeyboardMarkupDto.CreateFromButtonTexts("Отписка")
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