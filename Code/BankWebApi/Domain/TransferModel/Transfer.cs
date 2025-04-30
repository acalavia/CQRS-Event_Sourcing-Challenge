using BankWebApi.Domain.AccountModel;
using EventFlow.Entities;

namespace BankWebApi.Domain.TransferModel
{
    public class Transfer : Entity<TransferId>
    {
        
        public  Account FromAccount { get; set; }
        public  Account ToAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTransfer { get; set; }

        public Transfer(TransferId id, Account fromAccount, Account toAccount, decimal amount, DateTime dateTransfer) : base(id)
        {
             
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Amount = amount;
            DateTransfer = dateTransfer;
        }
    }
}
