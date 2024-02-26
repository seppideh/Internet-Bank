using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_Bank.Data
{
    public class Account
    {
        public int AccountId { get; set; }
        public AccountType AccountType { get; set; }
        public int Amount { get; set; }
        public string AccountNumber { get; set; }
        public string CardNumber { get; set; }
        public string CVV2 { get; set; }
        public string ExpireDate { get; set; }
        public string StaticPassword { get; set; }
        public bool IsBlocked { get; set; }
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Transaction> Transactions { get; set; }

    }
}