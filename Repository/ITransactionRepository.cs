using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internet_Bank.Data;
using Internet_Bank.Model.Transaction;

namespace Internet_Bank.Repository
{
    public interface ITransactionRepository
    {
        Task<DynamicCode> SendSMS(int userId, SendOtpDto model);
    }
}