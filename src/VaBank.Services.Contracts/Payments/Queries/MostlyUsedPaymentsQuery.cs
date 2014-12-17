using System;
using VaBank.Services.Contracts.Common.Queries;

namespace VaBank.Services.Contracts.Payments.Queries
{
    public class MostlyUsedPaymentsQuery : IRangeQuery<DateTime>, IUserQuery
    {
        public Guid UserId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int MaxResults { get; set; }
    }
}
