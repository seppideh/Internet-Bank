using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Internet_Bank.Data;
using Internet_Bank.Migrations;
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
                                                         x.CardNumber == model.SorceCardNumber &&
                                                         x.CVV2 == model.Cvv2 &&
                                                         x.ExpireDate == model.ExpireDate
                                                     ).FirstOrDefaultAsync();
            if (account != null)
            {
                var transaction = new Transaction()
                {
                    SorceCardNumber = model.SorceCardNumber,
                    Cvv2 = model.Cvv2,
                    ExpireDate = model.ExpireDate,
                    Amount = model.Amount,
                    DestinationCardNumber = model.DestinationCardNumber,
                    CreatedDateTime = DateTime.Now,
                    AccountId = account.AccountId,
                    TransactionStatus = false
                };

                _context.Add(transaction);
                await _context.SaveChangesAsync();

                int generatedId = transaction.TransactionId;
                // transaction.AccountId = account.AccountId;
                var nowTime = DateTime.Now;
                if (nowTime.Year < model.ExpireDate.Year || (nowTime.Year == model.ExpireDate.Year && nowTime.Month <= model.ExpireDate.Month))
                {
                    var random = new Random();
                    var randNumb = random.Next(10000, 100000);
                    Console.WriteLine(randNumb);


                    var dynamicCode = new DynamicCode()
                    {
                        Password = randNumb.ToString(),
                        CreatedAt = DateTime.Now,
                        ExpireAt = DateTime.Now.AddMinutes(1),
                        TransactionId = transaction.TransactionId
                    };
                    _context.Add(dynamicCode);
                    await _context.SaveChangesAsync();

                    // Sentd SMS
                    // var sender = "1000689696";
                    // var receptor = "09114458939";
                    // var message = $"{model.Amount} : مبلغ  {DateTime.Now.ToShortPersianDateString()} : تاریخ  {randNumb} : رمز یکبار مصرف  ";
                    // string ApiKey = Environment.GetEnvironmentVariable("Kavenegar_APIKey");
                    // var api = new Kavenegar.KavenegarApi(ApiKey);
                    // api.Send(sender, receptor, message);

                    return dynamicCode;
                }
                else
                {
                    transaction.Description = "کارت وارد شده منقضی شده است";
                }
            }
            // else
            // {
            //     transaction.Description = "اطلاعات کارت اشتباه است";
            // }

            // _context.Add(transaction);
            await _context.SaveChangesAsync();
            return new DynamicCode();
        }
    }
}