using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Internet_Bank.Validations;

namespace Internet_Bank.Model.Transaction
{
    public class SendOtpDto
    {
        [IsValidCardNumber]
        public string SourceCardNumber { get; set; }

        [IsValidCvv2]
        public string Cvv2 { get; set; }

        [IsValidExpireDate]
        public string ExpireDate { get; set; }

        [Range(1000, 5000000, ErrorMessage = "مبلغی بین 1000 و 5000000 تومان وارد کنید")]
        public int Amount { get; set; }

        [IsValidCardNumber]
        public string DestinationCardNumber { get; set; }
    }
}