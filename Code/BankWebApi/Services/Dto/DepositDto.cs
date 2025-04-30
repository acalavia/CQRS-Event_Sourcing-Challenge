using BankWebApi.Domain.AccountModel;

namespace BankWebApi.Services.Dto
{
    public class DepositDto
    {
        public decimal Amount { get; set; }
        public required AccountId AccountId { get; set; }
    }
}
