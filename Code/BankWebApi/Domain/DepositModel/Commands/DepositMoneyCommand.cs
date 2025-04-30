using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.AccountModel.Commands;
using EventFlow.Commands;

namespace BankWebApi.Domain.DepositModel.Commands
{
    public class DepositMoneyCommand : Command<AccountAggregate, AccountId>
    {
        public decimal Amount { get; set; }
        public Account Account { get; set; }

        public DepositMoneyCommand(AccountId aggregateId, Account account, decimal amount) : base(aggregateId)
        {
            Account = account;
            Amount = amount;
        }

    }

    public class DepositMoneyCommandHandler : CommandHandler<AccountAggregate, AccountId, DepositMoneyCommand>
    {
        public override Task ExecuteAsync(AccountAggregate aggregate, DepositMoneyCommand command, CancellationToken cancellationToken)
        {
            aggregate.MoneyDeposited(command.AggregateId, command.Account, command.Amount);
            // Logic to create account
            return Task.CompletedTask;
        }
    }

}
