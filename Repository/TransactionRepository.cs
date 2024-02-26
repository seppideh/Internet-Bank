using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Internet_Bank.Data;
// using Internet_Bank.Migrations;
using Internet_Bank.Model.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Internet_Bank.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly InternetBankContext _context;

        public TransactionRepository(InternetBankContext context)
        {
            _context = context;
        }

        public async Task<DynamicCode> SendSMS(int userId, SendOtpDto model)
        {

            var account = await _context.Accounts.Where(x => x.UserId == userId &&
                                                         x.CardNumber == model.SourceCardNumber &&
                                                         x.CVV2 == model.Cvv2 &&
                                                         x.ExpireDate == model.ExpireDate &&
                                                         x.IsBlocked == false
                                                       ).FirstOrDefaultAsync();

            if (account is null) return new DynamicCode(); ;

            var transaction = new Transaction()
            {
                SorceCardNumber = model.SourceCardNumber,
                Amount = model.Amount,
                DestinationCardNumber = model.DestinationCardNumber,
                CreatedDateTime = DateTime.Now,
                AccountId = account.AccountId,
                TransactionStatus = false
            };

            _context.Add(transaction);
            await _context.SaveChangesAsync();

            var dynamicPass = new Random().Next(10000, 100000);
            Console.WriteLine(dynamicPass);

            var dynamicCode = new DynamicCode()
            {
                Password = dynamicPass.ToString(),
                CreatedAt = DateTime.Now,
                ExpireAt = DateTime.Now.AddMinutes(1),
                TransactionId = transaction.TransactionId
            };
            _context.Add(dynamicCode);
            await _context.SaveChangesAsync();


            var user = await _context.Users.Where(x => x.Id == account.UserId).FirstOrDefaultAsync();
            var SorceCardNumber = model.SourceCardNumber.Replace(" ", "");
            var displayCardNumber = SorceCardNumber.Substring(0, 5) + "*******" + SorceCardNumber.Substring(11, 4);


            // Sentd SMS
            var sender = "1000689696";
            var receptor = user.PhoneNumber;
            var message = $"مبلغ : {model.Amount} \nتاریخ : {DateTime.Now.ToShortPersianDateString()}\nساعت :{DateTime.Now.TimeOfDay}\nشماره کارت : {displayCardNumber}\nرمز پویا : {dynamicPass}";
            string ApiKey = Environment.GetEnvironmentVariable("Kavenegar_APIKey");
            var api = new Kavenegar.KavenegarApi(ApiKey);
            // api.Send(sender, receptor, message);

            return dynamicCode;
        }

        
    }
}