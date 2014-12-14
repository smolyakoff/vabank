using System;
using System.Collections.Generic;
using PagedList;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Accounting.Commands;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Accounting.Queries;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Accounting
{
    public interface ICardAccountService : IService
    {
        CardLookupModel GetAccountingLookup();

        IList<CustomerCardModel> GetCustomerCards(IdentityQuery<Guid> userId);

        AccountBalanceModel GetCardBalance(CardBalanceQuery query);

        IPagedList<AccountBriefModel> GetCardAccounts(AccountQuery query);

        IList<UserCardModel> GetUserCards(CardQuery query); 
            
        IList<CardModel> GetAccountCards(IdentityQuery<string> accountNo);

        UserMessage CreateCard(CreateCardCommand command);

        UserMessage CreateCardAccount(CreateCardAccountCommand command);

        UserMessage SetCardBlock(SetCardBlockCommand command);

        UserMessage SetCardAssignment(SetCardAssignmentCommand command);

        UserMessage UpdateCardSettings(UpdateCardSettingsCommand command);

        CardAccountStatementModel GetCardAccountStatement(CardAccountStatementQuery query);

        AccountBriefModel GetCardAccountBrief(IdentityQuery<string> accountNo);
    }
}
