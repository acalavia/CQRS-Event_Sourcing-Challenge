using BankWebApi.Domain.TransactionModel.Events;
using BankWebApi.Domain.TransactionModel;
using EventFlow.ReadStores;
using BankWebApi.Domain.TransferModel.Events;
using BankWebApi.Domain.TransferModel;
using EventFlow.Aggregates;
using BankWebApi.Domain.AccountModel;

namespace BankWebApi.Domain.ReadModels
{
    public class TransferReadModel : IReadModel, IAmReadModelFor<TransferAggregate, TransferId, MoneyTransferredEvent>
    {
        public Transfer Transfer { get; set; }
        public HashSet<Transfer> Transfers { get; } = new HashSet<Transfer>();


        public Task ApplyAsync(IReadModelContext context, IDomainEvent<TransferAggregate, TransferId, MoneyTransferredEvent> domainEvent, CancellationToken cancellationToken)
        {
            Transfer = domainEvent.AggregateEvent.Transfer;
            Transfers.Add(domainEvent.AggregateEvent.Transfer);
            return Task.CompletedTask;
        }
    }
}
