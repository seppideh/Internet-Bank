using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internet_Bank.Model.Account;

namespace Internet_Bank.Service
{
    public class RandomService : IRandomService
    {
        private readonly Random _random;
        public RandomService()
        {
            _random = new Random();
        }

        public string AccountNumberGenerator(int userId, OpenAccountDto model)
        {
            var firstPart = _random.Next(10, 100).ToString();

            var secondPart = userId.ToString();
            if (secondPart.Length == 1) secondPart = "00" + secondPart;
            else if (secondPart.Length == 2) secondPart = "0" + secondPart;

            var thirdPart = _random.Next(1000, 10000).ToString();
            var fourthPart = (int)model.AccountType;

            return firstPart + "." + secondPart + "." + thirdPart + "." + fourthPart;
        }

        public string CardNumberGenerator()
        {
            int firstPart = _random.Next(1000, 10000);
            int secondPart = _random.Next(1000, 10000);
            int thirdPart = _random.Next(1000, 10000);
            int fourthPart = _random.Next(1000, 10000);

            return firstPart.ToString() + " " + secondPart.ToString() + " " + thirdPart.ToString() + " " + fourthPart.ToString();
        }
        public string Cvv2Generator()
        {
            return _random.Next(1000, 10000).ToString();
        }
        public string PasswordGenerator()
        {
            return _random.Next(100000, 1000000).ToString();
        }

    }
}