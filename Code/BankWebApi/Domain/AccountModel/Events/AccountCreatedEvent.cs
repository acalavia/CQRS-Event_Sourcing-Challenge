using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace BankWebApi.Domain.AccountModel.Events
{    
    public class AccountCreatedEvent : AggregateEvent<AccountAggregate, AccountId>
    {
        public Account Account { get; set; }
        public AccountCreatedEvent(Account acc)
        {
            Account = acc; 
        }
    }
}
