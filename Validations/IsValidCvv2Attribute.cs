using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_Bank.Validations
{
    public class IsValidCvv2Attribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var cvv2 = (string)value;

            if (cvv2.All(x => Char.IsDigit(x))
                        && cvv2.Length == 4)
            {
                return true;
            }

            return false;
        }
        public override string FormatErrorMessage(string name)
        {
            return "باید 4 رقمی باشد cvv2";
        }
    }
}