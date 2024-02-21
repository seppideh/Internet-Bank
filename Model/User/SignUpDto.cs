using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DNTPersianUtils.Core;

namespace Internet_Bank.Model.User
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "لطفا نام خود را وارد کنيد")]
        [RegularExpression(@"^[\u0600-\u06FF\s]+$", ErrorMessage = "لطفا نام خود را فارسي وارد کنيد")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "لطفا نام خانوادگي خود را وارد کنيد")]
        [RegularExpression(@"^[\u0600-\u06FF\s]+$", ErrorMessage = "لطفا نام خانوادگي خود را فارسي وارد کنيد")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "لطفا کدملي خود را وارد کنيد")]
        [ValidIranianNationalCode]
        public string NationalCode { get; set; }

        [Required(ErrorMessage = "لطفا تاريخ تولد خود را وارد کنيد")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "لطفا شماره موبايل خود را وارد کنيد")]
        [ValidIranianMobileNumber]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا ايميل خود را وارد کنيد")]
        [EmailAddress(ErrorMessage = "لطفا ايميل خود را با فرمت درست وارد کنيد")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا نام کاربری خود را وارد کنيد")]
        public string UserName { get; set; }
    }
}