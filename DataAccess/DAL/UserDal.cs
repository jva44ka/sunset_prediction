using System;

namespace DataAccess.DAL
{
    public class UserDal
    {
        public int Id { get; set; }

        public DialogStateDal? PreviousDialogState { get; set; }
        
        public DialogStateDal DialogState { get; set; }

        public int? CityId { get; set; }
        public DateTime StateChangeDate { get; set; }
    }
}
