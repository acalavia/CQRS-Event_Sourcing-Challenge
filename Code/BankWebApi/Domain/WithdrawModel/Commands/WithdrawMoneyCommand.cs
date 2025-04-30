using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.DepositModel.Commands;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace BankWebApi.Domain.WithdrawModel.Commands
{
    public class WithdrawMoneyCommand : Command<AccountAggregate, AccountId, IExecutionResult>
    {
        public decimal Amount { get; set; }
        public Account Account { get; set; }

        public WithdrawMoneyCommand(AccountId aggregateId, Account account, decimal amount) : base(aggregateId)
        {
            Account = account;
            Amount = amount;
        }
    }

    public class WithdrawMoneyCommandHandler : CommandHandler<AccountAggregate, AccountId, IExecutionResult, WithdrawMoneyCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(AccountAggregate aggregate, WithdrawMoneyCommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(aggregate.MoneyWithdrawn(command.AggregateId, command.Account, command.Amount));
        }
    }
}
