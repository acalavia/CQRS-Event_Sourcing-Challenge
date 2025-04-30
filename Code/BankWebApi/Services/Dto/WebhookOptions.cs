namespace BankWebApi.Services.Dto
{
    public class WebhookOptions
    {
        public string Uri { get; set; } = default!;
        public string ApiKey { get; set; } = default!;
        public string KeyParameter { get; set; } = default!;
        public string TitleParam { get; set; } = default!;
        public string ContentParam { get; set; } = default!;
    }
}
