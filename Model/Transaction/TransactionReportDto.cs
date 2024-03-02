using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_Bank.Model.Transaction
{
    public class TransactionReportDto
    {
        public int Amount { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int AccountId { get; set; }
        public string Description { get; set; }
        public string DestinationCardNumber { get; set; }
    }
}