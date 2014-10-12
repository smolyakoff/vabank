namespace VaBank.Data.Migrations
{
    internal class M1Profile
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public string SecretPhrase
        {
            get { return "Я люблю ВаБанк!"; }
        }
    }
}
