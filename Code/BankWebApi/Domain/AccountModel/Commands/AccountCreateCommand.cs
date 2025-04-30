using EventFlow.Commands;

namespace BankWebApi.Domain.AccountModel.Commands
{
    public class AccountCreateCommand : Command<AccountAggregate, AccountId>
    {
        //public AccountId IdAcc { get; set; }
        public string FirstName { get; set; }
        public decimal Balance { get; set; }

        public AccountCreateCommand(AccountId aggregateId, string fname, decimal balance) : base(aggregateId)
        {
            FirstName = fname;
            Balance = balance;         
        }
    }

    public class AccountCreateCommandHandler : CommandHandler<AccountAggregate, AccountId, AccountCreateCommand>
    {
        public override Task ExecuteAsync(AccountAggregate aggregate, AccountCreateCommand command, CancellationToken cancellationToken)
        {
            aggregate.CreateAccount(command.AggregateId, command.FirstName, command.Balance);
            // Logic to create account
            return Task.CompletedTask;
        }
    }
}
