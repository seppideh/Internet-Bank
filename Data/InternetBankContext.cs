using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Internet_Bank.Data
{
    public class InternetBankContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public InternetBankContext(DbContextOptions<InternetBankContext> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<DynamicCode> DynamicCodes { get; set; }
    }
}