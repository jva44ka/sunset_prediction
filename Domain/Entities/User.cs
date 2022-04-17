using System;
using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public int? CityId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; }

        public DialogState? PreviousDialogState { get; set; }
        public DialogState DialogState { get; set; }
        public DateTime StateChangeDate { get; set; }
    }
}
