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
    public interface ICardAccountService : IService
    {
        CardLookupModel GetAccountingLookup();

        IList<CustomerCardModel> GetCustomerCards(IdentityQuery<Guid> userId);

        AccountBalanceModel GetCardBalance(CardBalanceQuery query);

        IPagedList<CardAccountBriefModel> GetCardAccounts(AccountQuery query);

        IList<UserCardModel> GetUserCards(CardQuery query); 
            
        IList<CardModel> GetAccountCards(AccountCardsQuery query);

        UserMessage CreateCard(CreateCardCommand command);

        UserMessage CreateCardAccount(CreateCardAccountCommand command);

        UserMessage SetCardBlock(SetCardBlockCommand command);

        UserMessage SetCardActivation(SetCardActivationCommand command);

        UserMessage UpdateCardSettings(UpdateCardSettingsCommand command);

        CardAccountStatementModel GetCardAccountStatement(CardAccountStatementQuery query);

        CardAccountBriefModel GetCardAccountBrief(IdentityQuery<string> accountNo);
    }
}
