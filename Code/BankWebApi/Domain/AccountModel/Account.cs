using EventFlow.Entities;

namespace BankWebApi.Domain.AccountModel
{
    public class Account : Entity<AccountId>
    {

        public string FirstName { get; set; }
        public decimal Balance { get; set; }

        public Account(AccountId id, string firstName, decimal balance) : base(id)
        {
            //AccountId = id ;
            FirstName = firstName;
            Balance = balance;
        }

       
    }
 
}
