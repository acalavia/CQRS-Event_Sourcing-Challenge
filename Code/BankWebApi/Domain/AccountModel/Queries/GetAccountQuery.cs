using BankWebApi.Domain.ReadModels;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;
using System.Collections.Generic;
using System.Security.Principal;

namespace BankWebApi.Domain.AccountModel.Queries
{
    public class GetAccountQuery : IQuery<Account>
    {
        public GetAccountQuery(AccountId accountId)
        {
            AccountId = accountId;
        }

        public AccountId AccountId { get; }
    }

    public class GetAccountQueryHandler : IQueryHandler<GetAccountQuery, Account>
    {
        private readonly IInMemoryReadStore<AccountReadModel> _readStore;

        public GetAccountQueryHandler(IInMemoryReadStore<AccountReadModel> readStore)
        {
            _readStore = readStore;
        }
            

        async Task<Account?> IQueryHandler<GetAccountQuery, Account>.ExecuteQueryAsync(GetAccountQuery query, CancellationToken cancellationToken)
        { 
            var accountReadModels = await _readStore.FindAsync( rm => 
                                                                rm.Accounts.Any(account => 
                                                                                account.Id.Value == query.AccountId.Value),cancellationToken)
                                                    .ConfigureAwait(false);

            var accountReadModel = accountReadModels.First();

            // Convert AccountReadModel to Account
            var account = accountReadModel.Account;

            return await Task.FromResult(account);
        }
    }

}
