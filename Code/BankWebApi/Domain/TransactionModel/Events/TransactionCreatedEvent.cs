using BankWebApi.Domain.AccountModel;
using EventFlow.Aggregates;

namespace BankWebApi.Domain.TransactionModel.Events
{
    public class TransactionCreatedEvent : AggregateEvent<TransactionAggregate, TransactionId>
    {
        public Transaction Transaction { get; set; } 

        public TransactionCreatedEvent(Transaction transaction)
        {
            Transaction = transaction;
        }
       
    }
}
