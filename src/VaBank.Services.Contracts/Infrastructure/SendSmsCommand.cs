namespace VaBank.Services.Contracts.Infrastructure
{
    public class SendSmsCommand
    {
        public string PhoneNumber { get; set; }

        public string Text { get; set; }
    }
}
