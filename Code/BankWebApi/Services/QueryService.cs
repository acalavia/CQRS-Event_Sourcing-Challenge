
using EventFlow.Queries;

using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.AccountModel.Queries;

using BankWebApi.Domain.TransactionModel.Queries;
using BankWebApi.Domain.TransactionModel;
using BankWebApi.Domain.TransferModel;

namespace BankWebApi.Services
{
    public class QueryService : IQueryService
    {
        private readonly IQueryProcessor _queryProcessor;

        public QueryService(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
            
        }

        public async Task<Account?> GetBalance(AccountId AccountId, CancellationToken cancellationToken)
        {
            var account = await _queryProcessor.ProcessAsync(new GetAccountQuery(AccountId), cancellationToken).ConfigureAwait(false);
             
            return account;
        }

        public async Task<List<Transaction>> GetTransactionHistory(AccountId AccountId, CancellationToken cancellationToken)
        {
            var trans = await _queryProcessor.ProcessAsync(new GetTransactionHistoryQuery(AccountId), cancellationToken).ConfigureAwait(false);
            return trans.ToList();
        }

        public async Task<List<Account>> GetAllAccounts(CancellationToken cancellationToken)
        {
            var accounts = await _queryProcessor.ProcessAsync(new GetAllAccountsQuery(), cancellationToken).ConfigureAwait(false);
            return accounts.ToList();
        }

        //public async Task<List<Transfer>> GetAllTransfers(CancellationToken cancellationToken)
        //{
        //    var accounts = await _queryProcessor.ProcessAsync(new Get(), cancellationToken).ConfigureAwait(false);
        //    return accounts.ToList();
        //}
    }
}
