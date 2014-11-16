using VaBank.Services.Contracts.Processing;
using VaBank.Services.Contracts.Processing.Commands;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Processing
{
    public class CardTransferClientService : ICardTransferClientService
    {
        public BankOperationModel Transfer(PersonalCardTransferCommand command)
        {
            //create new CardTransfer in database and publish operation started event
            throw new System.NotImplementedException();
        }

        public BankOperationModel Transfer(InterbankCardTransferCommand command)
        {
            //create new CardTransfer in database and publish operation started event
            throw new System.NotImplementedException();
        }
    }
}
