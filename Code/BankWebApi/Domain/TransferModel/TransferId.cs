using BankWebApi.Domain.TransactionModel;
using EventFlow.Core;

namespace BankWebApi.Domain.TransferModel
{
    public class TransferId : Identity<TransferId>
    {
        public TransferId(string value) : base(value)
        {
        }

    }
   
}
