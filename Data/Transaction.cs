using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_Bank.Data
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string SorceCardNumber { get; set; }
        public int Amount { get; set; }
        public string DestinationCardNumber { get; set; }
        public bool TransactionStatus { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

    }
}