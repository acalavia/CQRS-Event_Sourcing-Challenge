using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.AccountModel.Commands;
using BankWebApi.Domain.TransactionModel;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using System.Transactions;

namespace BankWebApi.Domain.TransferModel.Commands
{
    public class TransferMoneyCommand : Command<TransferAggregate, TransferId, IExecutionResult>
    {
        public decimal Amount { get; }

        public Account ToAccount { get; }
        public Account FromAccount { get; } 
        public DateTime DateTransfer { get; set; }

        public TransferMoneyCommand(TransferId aggregateId, Account fromAccount, Account toAccount, decimal amount, DateTime date) : base(aggregateId)
        {
            Amount = amount;
            ToAccount = toAccount;
            
            FromAccount = fromAccount;
            DateTransfer = date;


        }
    }

    public class TransferMoneyCommandHandler : CommandHandler<TransferAggregate, TransferId, IExecutionResult, TransferMoneyCommand>
    {
       
        public override Task<IExecutionResult> ExecuteCommandAsync(TransferAggregate aggregate, TransferMoneyCommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(aggregate.TransferMoney(command.AggregateId , command.FromAccount, command.ToAccount, command.Amount, command.DateTransfer));
        }
    }
}
