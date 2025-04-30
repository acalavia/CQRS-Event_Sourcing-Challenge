using BankWebApi.Domain.AccountModel;
using EventFlow.Aggregates;

namespace BankWebApi.Domain.DepositModel.Events
{
    public class MoneyDepositedEvent : AggregateEvent<AccountAggregate, AccountId>
    {
        public decimal Amount { get; set; }
        public Account Account { get; set; }
        public MoneyDepositedEvent(Account account, decimal amount)
        {
            Account = account;
            Amount = amount;
        }
    }
}
