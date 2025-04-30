using BankWebApi.Domain.AccountModel.Events;
using BankWebApi.Domain.AccountModel;
using EventFlow.ReadStores;
using BankWebApi.Domain.TransactionModel.Events;
using EventFlow.Aggregates;
using BankWebApi.Domain.TransactionModel;

namespace BankWebApi.Domain.ReadModels
{
    public class TransactionReadModel : IReadModel, IAmReadModelFor<TransactionAggregate, TransactionId, TransactionCreatedEvent>
    {
        public Transaction Transaction { get; set; }
        public HashSet<Transaction> Transactions { get; } = new HashSet<Transaction>();

        public Task ApplyAsync(IReadModelContext context, IDomainEvent<TransactionAggregate, TransactionId, TransactionCreatedEvent> domainEvent, CancellationToken cancellationToken)
        {
            Transaction = domainEvent.AggregateEvent.Transaction;
            Transactions.Add(domainEvent.AggregateEvent.Transaction);
            return Task.CompletedTask;
        }

        public Transaction ToTransaction()
        {
            return Transaction;
        }
    }
}
