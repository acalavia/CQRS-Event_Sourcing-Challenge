using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.AccountModel.Commands;
using BankWebApi.Services.Dto;
using EventFlow.Aggregates.ExecutionResults;

namespace BankWebApi.Services
{
    public interface ICommandService
    {
        public Task<Account> CreateAccount(AccountDto accountDto, CancellationToken cancellationToken);

        public Task<IExecutionResult> TransferMoney(TransferDto transfer, CancellationToken cancellationToken);
        public Task<IExecutionResult> DepositMoney(DepositDto depositDto, CancellationToken cancellationToken);
        public Task<IExecutionResult> WithdrawMoney(WithdrawDto withdrawDto, CancellationToken cancellationToken);
    }
}
