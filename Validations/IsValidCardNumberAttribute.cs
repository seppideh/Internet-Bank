using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_Bank.Validations
{
    public class IsValidCardNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var cardNumber = (string)value;

            string[] parts = cardNumber.Split(' ');
            string cardNumberWithoutSpace = string.Join("", parts);


            if (cardNumberWithoutSpace.Length != 16)
            {
                return false;
            }

            foreach (var part in parts)
            {
                if (!part.All(x=> Char.IsDigit(x)) || part.Length!=4)
                {
                    return false;
                }
            }
            return true;
        }
        public override string FormatErrorMessage(string name)
        {
            return "شماره کارت وارد شده باید 16 رقمی باشد!";
        }
    }
}