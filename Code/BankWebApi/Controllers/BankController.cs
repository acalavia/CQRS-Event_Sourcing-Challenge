using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.AccountModel.Commands;
using BankWebApi.Services;
using BankWebApi.Services.Dto;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;


namespace BankWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankController : ControllerBase
    {
        private readonly ILogger<BankController> _logger;
        private readonly ICommandService _commandService;
        private readonly IQueryService _queryService;

        public BankController(ILogger<BankController> logger , ICommandService commandService, IQueryService queryService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
            _commandService = commandService;
            _queryService = queryService;
        }

        [HttpPost]
        [Route("accounts/newaccount")]
        public async Task<Account?> CreateAccount([FromBody] AccountDto accountDto, CancellationToken cancellationToken)
        {
            try
            {
                var newAccount = await _commandService.CreateAccount(accountDto, CancellationToken.None).ConfigureAwait(false);

                return newAccount; 
                //await Task.FromResult(Ok(newAccount)).ConfigureAwait(false);


            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)  
                _logger.LogError(ex, "Error creating account");
                return await Task.FromException<Account?>(ex);
            }
        }

        [HttpPost]
        [Route("accounts/transfer")]
        public async Task<IActionResult> TransferMoney([FromBody] TransferDto transfer, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _commandService.TransferMoney(transfer, CancellationToken.None).ConfigureAwait(false);

                if (result.IsSuccess)
                    return await Task.FromResult(Ok("Transfer money between accounts")).ConfigureAwait(false);
                else
                    return await Task.FromResult(Ok("Transfer not completed.")).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Transfering Money");
                // Log the exception (not implemented here)  
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("accounts/deposit")]
        public async Task<IActionResult> DepositMoney([FromBody] DepositDto depositDto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _commandService.DepositMoney(depositDto, CancellationToken.None).ConfigureAwait(false);

                return await Task.FromResult(Ok("Deposit money into an account")).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Deposit Money");
                // Log the exception (not implemented here)  
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("accounts/withdraw")]
        public async Task<IActionResult> WithdrawMoney([FromBody] WithdrawDto withdrawtDto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _commandService.WithdrawMoney(withdrawtDto, CancellationToken.None).ConfigureAwait(false);
                if (result.IsSuccess)
                    return await Task.FromResult(Ok("Withdraw money from an account")).ConfigureAwait(false);
                else
                    return await Task.FromResult(Ok("Insufficient funds an the account")).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Withdraw Money");
                // Log the exception (not implemented here)  
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("accounts/{id}/balance")]
        public async Task<ActionResult<decimal>> GetBalance([FromRoute] string id,CancellationToken cancellationToken)
        {
          
            AccountId idAcc = new AccountId(id);
            var account = await _queryService.GetBalance(idAcc, CancellationToken.None).ConfigureAwait(false);
            return Ok(account.Balance);
        }

        [HttpGet]
        [Route("accounts/{id}/history")]
        public async Task<ActionResult<List<Transaction>>> GetTransactionHistory([FromRoute] string id, CancellationToken cancellationToken)
        {            
            AccountId idAcc = new AccountId(id);
            var transactions = await _queryService.GetTransactionHistory(idAcc, CancellationToken.None).ConfigureAwait(false);
            return Ok(transactions);
        }
        [HttpGet]
        [Route("accounts")]
        public async Task<ActionResult<List<Account>>> GetAllAccounts ( CancellationToken cancellationToken)
        {
          
            var accounts = await _queryService.GetAllAccounts( CancellationToken.None).ConfigureAwait(false);
            return Ok(accounts);
        }

    }
}
