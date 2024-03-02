using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_Bank.Data
{
    public class DynamicCode
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpireAt { get; set; }
        public int UserId { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}