using BankWebApi.Domain.AccountModel.Queries;
using EventFlow.Jobs;
using EventFlow.Queries;
using Serilog;

namespace BankWebApi.Domain.AccountModel.Subscribers
{
    public class SendRequestBalanceUpdatedJob : IJob
    {
        private readonly IQueryProcessor _queryProcessor;

        public AccountId AccountId { get; set; }

        public SendRequestBalanceUpdatedJob(AccountId accountId )
        {
            AccountId = accountId;

        }
        

        public async Task ExecuteAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var _queryProcessor = serviceProvider.GetRequiredService<IQueryProcessor>();

            var query = new GetAccountQuery(AccountId);
            var account = await _queryProcessor.ProcessAsync(query, cancellationToken).ConfigureAwait(false);

            Log.Information($" Balance: {account.Balance}  of the accountId: {account.Id} has been updated");
            

            await Task.CompletedTask;
        }
    }
   
}
