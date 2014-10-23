using System;
using System.Collections.Generic;
using PagedList;
using VaBank.Services.Contracts.Accounting.Commands;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Accounting.Queries;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Common.Queries;

namespace VaBank.Services.Contracts.Accounting
{
    public interface IAccountManagementService
    {
        AccountingLookupModel GetLookup();

        IList<UserCardModel> GetUserCards(IdentityQuery<Guid> userId);

        IPagedList<CardAccountBriefModel> GetCardAccounts(CardAccountsQuery query);

        RequestedCardAccountModel RequestCardAccount(IdentityQuery<Guid> userId);

        UserMessage CreateCardAccount(CreateCardAccountCommand command);

        UserMessage SetCardBlock(SetCardBlockCommand command);

        UserMessage SetCardAssignment(SetCardAssignmentCommand command);

        UserMessage SetCardLimits(SetCardLimitsCommand command);
    }
}
