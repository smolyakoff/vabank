using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Util;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Membership.Factories
{
    [Injectable]
    public class UserPaymentProfileFactory
    {
        private readonly IQueryRepository<UserPaymentProfile> _paymentProfileRepository; 

        public UserPaymentProfileFactory(IQueryRepository<UserPaymentProfile> paymentProfileRepository)
        {
            Argument.NotNull(paymentProfileRepository, "paymentProfileRepository");

            _paymentProfileRepository = paymentProfileRepository;
        }

        public UserPaymentProfile Create(User user, string address, string fullName)
        {
            Argument.NotNull(user, "user");
            Argument.NotEmpty(address, "address");
            Argument.NotEmpty(fullName, "fullName");

            var payerTIN = Randomizer.NumericString(9);
            while (true)
            {
                var tin = payerTIN;
                var query = DbQuery.For<UserPaymentProfile>().FilterBy(x => x.PayerTIN == tin);
                if (_paymentProfileRepository.QueryOne(query) == null)
                {
                    break;
                }
                payerTIN = Randomizer.NumericString(9);
            }
            var profile = new UserPaymentProfile(user, fullName, payerTIN, address);
            return profile;
        }
    }
}
