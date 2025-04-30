using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.AccountModel.Events;
using BankWebApi.Domain.TransactionModel.Events;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankWebApi.Domain.TransactionModel
{
    public class TransactionAggregate : AggregateRoot<TransactionAggregate, TransactionId>
    {
        private readonly List<Transaction> _transactions = new List<Transaction>();
        public TransactionAggregate(TransactionId id) : base(id)
        {
        }
         
        public IExecutionResult CreateTransaction(Transaction transaction)
        {
            // Logic to create account
            Emit(new TransactionCreatedEvent(transaction));
            return ExecutionResult.Success();
        }

        public void Apply(TransactionCreatedEvent transactionCreatedEvent)
        {
            _transactions.Add( transactionCreatedEvent.Transaction);
        }
    }
}
