using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.TransactionModel;
using BankWebApi.Domain.TransferModel.Events;
using BankWebApi.Domain.WithdrawModel.Events;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using System.Security.Principal;

namespace BankWebApi.Domain.TransferModel
{
    public class TransferAggregate : AggregateRoot<TransferAggregate, TransferId>
    {
        private Transfer _transfer;
        public TransferAggregate(TransferId id) : base(id)
        {
        }

        public IExecutionResult TransferMoney(TransferId transferId, Account fromAccount, Account toAccount, decimal amount, DateTime dateTransfer)
        {
            // Logic to transfer money  
            if (toAccount.Balance - amount < 0)
                return ExecutionResult.Failed("Insufficient funds");
            else
            {
                var transfer = new Transfer(transferId, fromAccount, toAccount, amount, dateTransfer);
                Emit(new MoneyTransferredEvent(transfer));
                return ExecutionResult.Success();
            }
        }
        public void Apply(MoneyTransferredEvent transferMoneyEvent)
        {
            _transfer = transferMoneyEvent.Transfer;
        }
    }
}
