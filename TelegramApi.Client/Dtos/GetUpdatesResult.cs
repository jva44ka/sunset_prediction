namespace TelegramApi.Client.Dtos
{
    public class GetUpdatesResult
    {
        public bool Ok { get; set; }
        public UpdateDto[] Result { get; set; }
    }
}
