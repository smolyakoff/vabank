namespace VaBank.Services.Infrastructure.Sms
{
    public class SmsModel
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Text { get; set; }

        public override string ToString()
        {
            return string.Format("[From: {0} - To: {1}] {2}", From, To, Text);
        }
    }
}
