using System;
using Domain.Entities;
using Domain.Services.Interfaces;

namespace Domain.Services
{

    public class DialogStateService : IDialogStateService
    {
        private readonly ICitiesParserService _citiesParserService;

        public DialogStateService(ICitiesParserService citiesParserService)
        {
            _citiesParserService = citiesParserService;
        }

        public DialogState TransitionState(DialogState currentState, string message)
        {

            return currentState;
        }
    }
}