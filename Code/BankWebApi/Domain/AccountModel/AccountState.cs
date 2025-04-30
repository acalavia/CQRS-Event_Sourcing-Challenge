using BankWebApi.Domain.AccountModel.Events;
using EventFlow.Aggregates;

namespace BankWebApi.Domain.AccountModel
{
    public class AccountState : AggregateState<AccountAggregate, AccountId, AccountState>,  
                                    IApply<AccountCreatedEvent>
    {
        public string FirstName { get; set; } = default!;
        public decimal Balance { get; set; }

        
        public void Apply(AccountCreatedEvent aggregateEvent)
        {
            FirstName = aggregateEvent.Account.FirstName;
            Balance = aggregateEvent.Account.Balance;
        }
    }

}
