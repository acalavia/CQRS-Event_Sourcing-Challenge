using BankWebApi.Domain.AccountModel; 
using BankWebApi.Domain.TransactionModel;
using BankWebApi.Domain.TransferModel;

namespace BankWebApi.Services
{
    public interface  IQueryService
    {
        public Task<Account?> GetBalance(AccountId AccountId, CancellationToken cancellationToken);

        public Task<List<Account>> GetAllAccounts(CancellationToken cancellationToken);

        public Task<List<Transaction>> GetTransactionHistory(AccountId AccountId, CancellationToken cancellationToken);
        //public Task<List<Transfer>> GetAllTransfers( CancellationToken cancellationToken);

    }
}
