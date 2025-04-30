using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.TransactionModel;

namespace BankWebApi.Services.Dto
{
    public class TransferDto
    {
        public required AccountId FromAccountId { get; init; }
        public required AccountId ToAccountId { get; init; }
        public required decimal Amount { get; init; }
    }
}
