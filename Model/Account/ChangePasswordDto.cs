using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_Bank.Model.Account
{
    public class ChangePasswordDto
    {
        public int AccountId { get; set; }
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [Compare("RepeatNewPassword")]
        public string NewPassword { get; set; }

        [Required]
        public string RepeatNewPassword { get; set; }
    }
}