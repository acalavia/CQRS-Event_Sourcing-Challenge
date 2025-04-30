using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.AccountModel.Commands;
using EventFlow.Commands;

namespace BankWebApi.Domain.TransactionModel.Commands
{
    public class TransactionCreatedCommand : Command<TransactionAggregate, TransactionId>
    {
        public AccountId AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTransaction { get; set; }
        public TransactionType TypeTransaction { get; set; }
        public AccountId? SourceAccountId { get; set; }
        public decimal Balance { get; set; }

        public TransactionCreatedCommand(TransactionId aggregateId, AccountId accountId, decimal amount, DateTime date, TransactionType type, AccountId? sourceAccountId, decimal balance) : base(aggregateId)
        {
            AccountId = accountId;
            Amount = amount;
            DateTransaction = date;
            TypeTransaction = type;
            SourceAccountId = sourceAccountId;
            Balance = balance;
        }
    }

    public class TransactionCreatedCommandHandler : CommandHandler<TransactionAggregate, TransactionId, TransactionCreatedCommand>
    {
        public override Task ExecuteAsync(TransactionAggregate aggregate, TransactionCreatedCommand command, CancellationToken cancellationToken)
        {
            Transaction trans = new Transaction(command.AggregateId, command.AccountId, command.SourceAccountId, command.Amount, command.DateTransaction, command.TypeTransaction, command.Balance);
            aggregate.CreateTransaction(trans);
            // Logic to create account
            return Task.CompletedTask;
        }
    }
}
