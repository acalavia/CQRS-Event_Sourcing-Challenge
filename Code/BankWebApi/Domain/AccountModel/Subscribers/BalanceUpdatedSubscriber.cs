using BankWebApi.Domain.AccountModel.Queries;
using BankWebApi.Domain.DepositModel.Events;
using BankWebApi.Domain.WithdrawModel.Events;
using BankWebApi.Services;
using EventFlow.Aggregates;
using EventFlow.Jobs;
using EventFlow.Queries;
using EventFlow.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BankWebApi.Domain.AccountModel.Subscribers
{
    public class BalanceUpdatedSubscriber : 
                            ISubscribeSynchronousTo<AccountAggregate, AccountId, MoneyDepositedEvent> ,
                            ISubscribeSynchronousTo<AccountAggregate, AccountId, MoneyWithdrawnEvent>
    {
        private readonly IJobScheduler _jobScheduler;
        private readonly WebhookService _webhookService;

        public BalanceUpdatedSubscriber(IJobScheduler jobScheduler, WebhookService webhookService)
        {
            _jobScheduler = jobScheduler;
            _webhookService = webhookService;
        }
        public async Task HandleAsync(IDomainEvent<AccountAggregate, AccountId, MoneyDepositedEvent> domainEvent, CancellationToken cancellationToken)
        { 
            decimal balanceUpd= domainEvent.AggregateEvent.Account.Balance + domainEvent.AggregateEvent.Amount;

            string message = $" The balance of accountId {domainEvent.AggregateEvent.Account.Id} has been updated to {balanceUpd} (+{domainEvent.AggregateEvent.Amount})";
            Log.Information(message);

            //// await _webhookService.SendHook("Balance Update", message);
            // var job = new SendRequestBalanceUpdatedJob(domainEvent.AggregateEvent.Account.Id); 
           // return Task.CompletedTask; // _jobScheduler.ScheduleAsync(job, TimeSpan.FromSeconds(5), cancellationToken);
        }

        public async Task HandleAsync(IDomainEvent<AccountAggregate, AccountId, MoneyWithdrawnEvent> domainEvent, CancellationToken cancellationToken)
        {
            decimal balanceUpd = domainEvent.AggregateEvent.Account.Balance -+ domainEvent.AggregateEvent.Amount;

            string message = $" The balance of accountId {domainEvent.AggregateEvent.Account.Id} has been updated to {balanceUpd} (-{domainEvent.AggregateEvent.Amount})";
 
            Log.Information(message);
            await _webhookService.SendHook("Balance Update", message);

            //var job = new SendRequestBalanceUpdatedJob(domainEvent.AggregateEvent.Account.Id);
            //return Task.CompletedTask;  //_jobScheduler.ScheduleAsync(job, TimeSpan.FromSeconds(5), cancellationToken);
        }
    }
}
