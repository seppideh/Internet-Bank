using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internet_Bank.Data;

namespace Internet_Bank.Model.Account
{
    public class GetDetailsOfAccount
    {
        public string AccountNumber { get; set; }
        public string CardNumber { get; set; }
        public string CVV2 { get; set; }
        public string ExpireDate { get; set; }
        public string StaticPassword { get; set; }
        public int AccountId { get; set; }
        public AccountType AccountType { get; set; }

    }
}