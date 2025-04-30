using BankWebApi.Domain.ReadModels;
using BankWebApi.Domain.TransactionModel.Queries;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;

namespace BankWebApi.Domain.AccountModel.Queries
{
    public class GetAllAccountsQuery : IQuery<IReadOnlyCollection<Account>>
    {
        public GetAllAccountsQuery()
        {
        }

        public class GetAllAccountsQueryHandler : IQueryHandler<GetAllAccountsQuery, IReadOnlyCollection<Account>>
        {
            private readonly IInMemoryReadStore<AccountReadModel> _readStore;

            public GetAllAccountsQueryHandler(
                IInMemoryReadStore<AccountReadModel> readStore)
            {
                _readStore = readStore;
            }

            public async Task<IReadOnlyCollection<Account>> ExecuteQueryAsync(GetAllAccountsQuery query, CancellationToken cancellationToken)
            {
                var accountReadModels = await _readStore.FindAsync(rm => true, cancellationToken).ConfigureAwait(false);
                return accountReadModels.Select(rm => rm.ToAccount()).ToList();

            }
        }
    }
}
