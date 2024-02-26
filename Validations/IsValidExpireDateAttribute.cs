using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DNTPersianUtils.Core;

namespace Internet_Bank.Validations
{
    public class IsValidExpireDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string expireDate)
            {
                // DateTime result;
                // DateTime.TryParse(expireDate, out result);
                // if (result >= DateTime.Now) return true;
                var now = DateTime.Now.ToString("yy/MM");
                string[] nowYearMonth = now.Split("/");
                int nowYear = Convert.ToInt32(nowYearMonth[0]);
                int nowMonth = Convert.ToInt32(nowYearMonth[1]);

                string[] expireYearMonth = expireDate.Split("/");
                int expireYear = Convert.ToInt32(expireYearMonth[0]);
                int expireMonth = Convert.ToInt32(expireYearMonth[1]);


                if (nowYear < expireYear || (nowYear == expireYear && nowMonth < expireMonth))
                {
                    return true;
                }
            }
            return false;
        }
        public override string FormatErrorMessage(string name)
        {
            return "کارت وارد شده منقضی شده است";
        }
    }
}