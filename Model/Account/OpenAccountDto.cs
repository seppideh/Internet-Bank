using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Internet_Bank.Data;

namespace Internet_Bank.Model.Account
{
    public class OpenAccountDto
    {
        [Required]
        public AccountType AccountType { get; set; }

        [Required]
        [Range(10000,int.MaxValue,ErrorMessage ="حداقل مبلغ برای افتتاح حساب بایستی 10000 تومان باشد")]
        public int Amount { get; set; }
    }
}