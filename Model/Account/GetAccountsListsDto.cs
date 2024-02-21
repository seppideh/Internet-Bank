using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_Bank.Model.Account
{
    public class GetAccountsListsDto
    {
        public string AccountNumber { get; set; }
        public int AccountId { get; set; }
        public string CardNumber { get; set; }
    }
}