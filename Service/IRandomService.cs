using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internet_Bank.Model.Account;

namespace Internet_Bank.Service
{
    public interface IRandomService
    {
        string AccountNumberGenerator(int userId, OpenAccountDto model);
        string CardNumberGenerator();
        string Cvv2Generator();
        public string PasswordGenerator();
    }
}