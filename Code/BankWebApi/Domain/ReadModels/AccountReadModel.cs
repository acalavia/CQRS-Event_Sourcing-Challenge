using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.AccountModel.Events;
using BankWebApi.Domain.DepositModel.Events;
using BankWebApi.Domain.WithdrawModel.Events;
using EventFlow.Aggregates;
using EventFlow.ReadStores;

namespace BankWebApi.Domain.ReadModels
{
    public class AccountReadModel : IReadModel, 
                IAmReadModelFor<AccountAggregate, AccountId, AccountCreatedEvent>,
                IAmReadModelFor<AccountAggregate, AccountId, MoneyDepositedEvent>,
                IAmReadModelFor<AccountAggregate, AccountId, MoneyWithdrawnEvent>
    {
        public Account Account { get; set; }

        public decimal Balance { get; private set; }

        public HashSet<Account> Accounts { get; } = new HashSet<Account>();

        public Task ApplyAsync( IReadModelContext context, 
                                IDomainEvent<AccountAggregate, AccountId, AccountCreatedEvent> domainEvent, CancellationToken cancellationToken)
        {
            Account = domainEvent.AggregateEvent.Account;
            Accounts.Add (domainEvent.AggregateEvent.Account);
            return Task.CompletedTask;
        }

        public Task ApplyAsync(IReadModelContext context, IDomainEvent<AccountAggregate, AccountId, MoneyDepositedEvent> domainEvent, CancellationToken cancellationToken)
        {
            Account.Balance += domainEvent.AggregateEvent.Amount;
            return Task.CompletedTask;
        }

        public Task ApplyAsync(IReadModelContext context, IDomainEvent<AccountAggregate, AccountId, MoneyWithdrawnEvent> domainEvent, CancellationToken cancellationToken)
        {
            
            Account.Balance -= domainEvent.AggregateEvent.Amount;
            return Task.CompletedTask;
                         
        }

        public Account ToAccount()
        {
            return Account;
        }
    }
  
}
