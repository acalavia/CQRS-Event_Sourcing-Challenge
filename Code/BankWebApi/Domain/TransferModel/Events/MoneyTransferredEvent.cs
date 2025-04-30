using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.TransactionModel;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace BankWebApi.Domain.TransferModel.Events
{
    [EventVersion("TransferMoney", 1)]
    public class MoneyTransferredEvent : AggregateEvent<TransferAggregate, TransferId>
    {
    
        public Transfer Transfer { get; set; }

        public MoneyTransferredEvent(Transfer  transfer)
        {          
            Transfer = transfer;
        }
    }
}
