using BankWebApi.Domain.AccountModel.Events;
using BankWebApi.Domain.DepositModel.Events;
using BankWebApi.Domain.TransactionModel;
using BankWebApi.Domain.TransactionModel.Events;
using BankWebApi.Domain.WithdrawModel.Events;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
 

namespace BankWebApi.Domain.AccountModel
{
    public class AccountAggregate : AggregateRoot<AccountAggregate, AccountId>
    {
        private readonly List<Account> _accounts = new List<Account>();
        private Account _account;
        public AccountAggregate(AccountId id) : base(id)
        {
        }

        public IExecutionResult CreateAccount(AccountId id, string firstName, decimal balance)
        {
            // Logic to create account
            Account acc = new Account(id, firstName, balance);
            Emit(new AccountCreatedEvent(acc));
            return ExecutionResult.Success();
        }

        public void Apply(AccountCreatedEvent accountCreatedEvent)
        {
            _account = accountCreatedEvent.Account;
            _accounts.Add(accountCreatedEvent.Account);
        }


        public IExecutionResult MoneyDeposited(AccountId id, Account account, decimal amount)
        {           

            Emit(new MoneyDepositedEvent(account, amount));
            return ExecutionResult.Success();
        }

        public void Apply(MoneyDepositedEvent moneyDepositedEvent)
        {
            _account = moneyDepositedEvent.Account;
            
        }

        public IExecutionResult MoneyWithdrawn(AccountId id, Account account, decimal amount)
        {
            if (account.Balance - amount < 0)
            {
                return ExecutionResult.Failed("Insufficient funds");
            }
           
            Emit(new MoneyWithdrawnEvent(account, amount));
            return ExecutionResult.Success();
            
            
        }

        public void Apply(MoneyWithdrawnEvent moneyWithdrawnEvent)
        {
            _account = moneyWithdrawnEvent.Account;            
        }
        

    }
}
