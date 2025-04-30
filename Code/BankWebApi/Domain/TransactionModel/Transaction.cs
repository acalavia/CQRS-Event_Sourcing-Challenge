using BankWebApi.Domain.AccountModel;
using EventFlow.Entities;

namespace BankWebApi.Domain.TransactionModel
{
    public class Transaction : Entity<TransactionId>
    {
        public AccountId AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTransaction { get; set; }
        public TransactionType TypeTransaction { get; set; }
        public string TypeTransactionDescr { get; set; }
        public AccountId? SourceAccountId { get; set; }
        public decimal Balance { get; set; }


        public Transaction(TransactionId id, AccountId accountId, AccountId sourceAccountId, decimal amount, DateTime dateTransaction, TransactionType type, decimal balance) : base(id)
        {
            AccountId = accountId;
            Amount = amount;
            DateTransaction = dateTransaction;
            TypeTransaction = type;
            TypeTransactionDescr = type.ToString();
            SourceAccountId = sourceAccountId;
            Balance = balance;
        }
    }
}
