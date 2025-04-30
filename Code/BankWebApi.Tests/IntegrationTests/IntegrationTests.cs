using BankWebApi.Domain.AccountModel.Commands;
using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.AccountModel.Queries;
using BankWebApi.Domain.ReadModels;
using BankWebApi.Domain.TransactionModel.Queries;

using EventFlow;
using EventFlow.Queries;
using EventFlow.Aggregates.ExecutionResults;

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using EventFlow.Extensions;
using System;
using BankWebApi.Domain.AccountModel.Events;
using NUnit.Framework;

namespace BankWebApi.Tests.IntegrationTests
{
    [Category("Integration")]
    public class IntegrationTests
    {
        public ICommandBus _commandBus { get; set; }

        [Fact]
        public async Task ReadModelTest()
        {

            var resolver = EventFlowOptions.New()
                            .UseInMemoryReadStoreFor<AccountReadModel>()
                            .UseInMemoryReadStoreFor<TransactionReadModel>()
                            .AddQueryHandlers(new[] {
                                      typeof(GetAccountQueryHandler),
                                      typeof(GetTransactionHistoryQueryHandler)
                            })

                            .AddCommands(typeof(AccountCreateCommand))
                            .AddCommandHandlers(typeof(AccountCreateCommandHandler))
                            .AddEvents(typeof(AccountCreatedEvent))

                            .ServiceCollection.BuildServiceProvider();

            _commandBus = resolver.GetRequiredService<ICommandBus>();

            //create new Account
            var accountId = AccountId.New;
            string firstName = "John Doe";
            decimal balance = 1000.00m;
            var command = new AccountCreateCommand(accountId, firstName, balance);
            IExecutionResult result = await _commandBus.PublishAsync(command, CancellationToken.None);

            var queryProcessor = resolver.GetRequiredService<IQueryProcessor>();

            AccountReadModel accountReadModel = await queryProcessor.ProcessAsync(
                                                                             new ReadModelByIdQuery<AccountReadModel>(accountId),
                                                                             CancellationToken.None)
                                                                    .ConfigureAwait(false);

            accountReadModel.Should().NotBeNull();
            accountReadModel.Account.Should().NotBeNull(); // Ensure Account is not null 
            accountReadModel.Account.Balance.Should().Be(1000);
        }
    }
}
