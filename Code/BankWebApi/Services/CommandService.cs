using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.AccountModel.Commands;
using BankWebApi.Domain.AccountModel.Queries;
using BankWebApi.Domain.DepositModel.Commands;
using BankWebApi.Domain.TransactionModel;
using BankWebApi.Domain.TransactionModel.Commands;
using BankWebApi.Domain.TransferModel;
using BankWebApi.Domain.TransferModel.Commands;
using BankWebApi.Domain.WithdrawModel.Commands;
using BankWebApi.Services.Dto;
using EventFlow;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Queries;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankWebApi.Services
{
    public class CommandService : ICommandService
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;
        public CommandService( ICommandBus commandBus, IQueryProcessor queryProcessor)
        {          
            _commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }
        public async Task<Account> CreateAccount(AccountDto accountDto, CancellationToken cancellationToken)
        {
            // create new Account
            var accountId = AccountId.New;
            var command = new AccountCreateCommand(accountId, accountDto.FirstName, accountDto.Balance );
            await _commandBus.PublishAsync(command, cancellationToken);

            // save transaction of created account
            await SaveTransaction(command.AggregateId, command.Balance, null, TransactionType.created, command.Balance, cancellationToken);
          
            return new Account (command.AggregateId, command.FirstName ,command.Balance) ;
        }


        public async Task<IExecutionResult> TransferMoney(TransferDto transfer, CancellationToken cancellationToken)
        {

            // Get Account
            var accountFrom = await _queryProcessor.ProcessAsync(new GetAccountQuery(transfer.FromAccountId), cancellationToken).ConfigureAwait(false);
            if (accountFrom != null)
            {
                // Get Account
                var accountTo = await _queryProcessor.ProcessAsync(new GetAccountQuery(transfer.ToAccountId), cancellationToken).ConfigureAwait(false);
                if (accountTo != null) 
                {
                    if (!accountFrom.Id.Equals(accountTo.Id))
                    {
                        DateTime date = DateTime.UtcNow;
                        var transferId = TransferId.New;
                        var command = new TransferMoneyCommand(transferId, accountFrom, accountTo, transfer.Amount, date);

                        var result = await _commandBus.PublishAsync(command, cancellationToken).ConfigureAwait(false);

                        if (result.IsSuccess)
                        {
                            // save transaction of withdraw Money
                            var withdraw = new WithdrawMoneyCommand(accountFrom.Id, accountFrom, transfer.Amount);
                            var result2 = await _commandBus.PublishAsync(withdraw, cancellationToken);

                            if (result2.IsSuccess)
                            {
                                // save transaction of Withdraw  Money
                                await SaveTransaction(withdraw.AggregateId, -withdraw.Amount, accountTo.Id, TransactionType.transfer, accountFrom.Balance, cancellationToken);

                                // save transaction of Deposit  Money

                                var deposit = new DepositMoneyCommand(accountTo.Id, accountTo, transfer.Amount);
                                var result3 = await _commandBus.PublishAsync(deposit, cancellationToken);
                                // save transaction of Deposit  Money
                                await SaveTransaction(deposit.AggregateId, deposit.Amount, accountFrom.Id, TransactionType.transfer, accountTo.Balance, cancellationToken);

                                return result3;

                            }
                            else
                                return ExecutionResult.Failed("Transfer not completed.");

                        }
                        else
                            return ExecutionResult.Failed("Transfer not completed.");
                    }
                    else
                        return ExecutionResult.Failed($"you can't make a transfer to the same account.");
                }
                else
                    return ExecutionResult.Failed($"Account with ID {transfer.ToAccountId} not found.");
            }
            else
                return ExecutionResult.Failed($"Account with ID {transfer.FromAccountId} not found."); 


        }


        public async Task<IExecutionResult> DepositMoney(DepositDto depositDto, CancellationToken cancellationToken)
        {
            // Get Account
            var account = await _queryProcessor.ProcessAsync(new GetAccountQuery(depositDto.AccountId), cancellationToken).ConfigureAwait(false);
            if (account == null)
            {
                throw new ArgumentException($"Account with ID {depositDto.AccountId} not found.");
            }
            else
            {
                // create new Deposit
                var command = new DepositMoneyCommand(depositDto.AccountId, account,  depositDto.Amount);

                // save transaction of Deposit  Money
                await SaveTransaction(command.AggregateId, command.Amount, null, TransactionType.deposit, account.Balance + depositDto.Amount, cancellationToken);
                
                return await _commandBus.PublishAsync(command, cancellationToken);
            }
             
        }

        public async Task<IExecutionResult> WithdrawMoney(WithdrawDto withdrawDto, CancellationToken cancellationToken)
        {

            // Get Account
            var account = await _queryProcessor.ProcessAsync(new GetAccountQuery(withdrawDto.AccountId), cancellationToken).ConfigureAwait(false);
            
            if (account == null)
            {
                throw new ArgumentException($"Account with ID {withdrawDto.AccountId} not found.");
            }
            else
            {
                var command = new WithdrawMoneyCommand(withdrawDto.AccountId, account, withdrawDto.Amount );

                // save transaction of Withdraw  Money
                await SaveTransaction(command.AggregateId, -command.Amount, null, TransactionType.withdraw, account.Balance - withdrawDto.Amount, cancellationToken);

                return await _commandBus.PublishAsync(command, cancellationToken).ConfigureAwait(false);
            }
         
        }



        private async Task SaveTransaction(AccountId accountId, decimal amount, AccountId? sourceAccountId, TransactionType type, decimal balance, CancellationToken cancellationToken)
        {
            var transactionId = TransactionId.New;
            var transCommand = new TransactionCreatedCommand(transactionId, accountId, amount, DateTime.UtcNow, type, sourceAccountId, balance);
            await _commandBus.PublishAsync(transCommand, cancellationToken);

        }

    }

}
