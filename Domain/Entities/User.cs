using System;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public DialogState? PreviousDialogState { get; set; }

        public DialogState DialogState { get; set; }

        public int? CityId { get; set; }
        public DateTime StateChangeDate { get; set; }
    }
}
