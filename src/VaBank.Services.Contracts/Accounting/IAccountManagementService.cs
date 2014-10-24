using System;
using System.Collections.Generic;
using PagedList;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Accounting.Commands;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Accounting.Queries;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Accounting
{
    public interface IAccountManagementService
    {
        AccountingLookupModel GetAccountingLookup();

        IList<UserCardModel> GetUserCards(IdentityQuery<Guid> userId);

        IPagedList<AccountBriefModel> GetCardAccounts(AccountQuery query);

        IList<CardModel> GetCards(IdentityQuery<string> accountNo);
            
        //RequestedCardAccountModel RequestCardAccount(IdentityQuery<Guid> userId);

        UserMessage CreateCard(CreateCardCommand command);

        UserMessage CreateCardAccount(CreateCardAccountCommand command);

        UserMessage SetCardBlock(SetCardBlockCommand command);

        UserMessage SetCardAssignment(SetCardAssignmentCommand command);

        UserMessage SetCardLimits(SetCardLimitsCommand command);
    }
}
