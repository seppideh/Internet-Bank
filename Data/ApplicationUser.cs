using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Internet_Bank.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        public override int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public DateTime Birthdate { get; set; }
        public ICollection<Account> Accounts { get; set; }

    }
}