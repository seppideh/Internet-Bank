using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_Bank.Validations
{
    public class IsValidUserAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;
        public IsValidUserAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }
        public override bool IsValid(object value)
        {
            if (value is DateTime birthDate)
            {
                if (birthDate.AddYears(_minimumAge) > DateTime.Now) return false;
            }
            return true;
        }
        public override string FormatErrorMessage(string name)
        {
            return $"سن شما باید حداقل {_minimumAge}سال باشد!";
        }
    }
}