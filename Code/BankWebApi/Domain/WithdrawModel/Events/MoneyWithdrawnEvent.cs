using BankWebApi.Domain.AccountModel;
using EventFlow.Aggregates;

namespace BankWebApi.Domain.WithdrawModel.Events
{
    public class MoneyWithdrawnEvent : AggregateEvent<AccountAggregate, AccountId>
    {
        public decimal Amount { get; set; }
        public Account Account { get; set; }
        public MoneyWithdrawnEvent(Account account, decimal amount)
        {
            Account = account;
            Amount = amount;
        }
    }
}
