using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace BankWebApi.Domain.TransactionModel
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class TransactionId : Identity<TransactionId>
    {
        public TransactionId(string value) : base(value)
        {
        }
     
    }
   
     
}
