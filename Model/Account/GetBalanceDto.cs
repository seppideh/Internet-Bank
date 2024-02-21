using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_Bank.Model.Account
{
    public class GetBalanceDto
    {
        public int AccountId { get; set; }
        public int Amount { get; set; }
        public string AccountNumber { get; set; }
    }
}