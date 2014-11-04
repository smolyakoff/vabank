namespace VaBank.Services.Contracts.Infrastructure.Sms
{
    public class SendSmsCommand
    {
        public string RecipientPhoneNumber { get; set; }

        public string Text { get; set; }

        public override string ToString()
        {
            return string.Format("For [{0}]: {1}", RecipientPhoneNumber, Text);
        }
    }
}
