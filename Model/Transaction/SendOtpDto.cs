using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_Bank.Model.Transaction
{
    public class SendOtpDto
    {
        [Required]
        public string SorceCardNumber { get; set; }
        public string Cvv2 { get; set; }
        public DateTime ExpireDate { get; set; }

        [Range(1000,5000000,ErrorMessage ="مبلغی بین 1000 و 5000000 تومان وارد کنید")]
        public int Amount { get; set; }
        public string DestinationCardNumber { get; set; }
    }
}