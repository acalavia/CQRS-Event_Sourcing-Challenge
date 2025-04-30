using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.ReadModels;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;

namespace BankWebApi.Domain.TransactionModel.Queries
{
    public class GetTransactionHistoryQuery : IQuery<IReadOnlyCollection<Transaction>>
    {
   
        public GetTransactionHistoryQuery(AccountId accountId)
        {
            AccountCompId = accountId;
        
        }
        public AccountId AccountCompId { get; }
    }

    public class GetTransactionHistoryQueryHandler : IQueryHandler<GetTransactionHistoryQuery, IReadOnlyCollection<Transaction>>
    {
        private readonly IInMemoryReadStore<TransactionReadModel> _readStore;

        public GetTransactionHistoryQueryHandler(
            IInMemoryReadStore<TransactionReadModel> readStore)
        {
            _readStore = readStore;
        }

        public async Task<IReadOnlyCollection<Transaction>> ExecuteQueryAsync(GetTransactionHistoryQuery query, CancellationToken cancellationToken)
        {
            var transactionReadModels = await _readStore.FindAsync(rm =>
                                                                   rm.Transactions.Any(ac =>
                                                                                       ac.AccountId.Value == query.AccountCompId.Value), cancellationToken)
                                                        .ConfigureAwait(false);

            return transactionReadModels.Select(rm => rm.ToTransaction()).ToList();
             
        }
    }
}
