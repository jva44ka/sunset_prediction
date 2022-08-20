namespace TelegramApi.Client.Dtos
{
    public class GetUpdatesResult
    {
        public bool Ok { get; set; }
        public Update[] Result { get; set; }
    }
}
