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
                AccountId = account.Id,
                Status = false
            };

            _context.Add(transaction);
            await _context.SaveChangesAsync();

            var dynamicPass = new Random().Next(10000, 100000);
            Console.WriteLine(dynamicPass);

            var dynamicCode = new DynamicCode()
            {
                Password = dynamicPass.ToString(),
                CreatedAt = DateTime.Now,
                ExpireAt = DateTime.Now.AddMinutes(3),
                TransactionId = transaction.Id,
                UserId = userId
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

        public async Task<bool> TransferMoney(int userId, TransferMoneyDto model)
        {
            var dynamicCode = await _context.DynamicCodes.Where(x => x.Password == model.Password
                                                                && x.UserId == userId
                                                                ).FirstOrDefaultAsync();

            var transaction = await _context.Transactions.Where(x => x.Id == dynamicCode.TransactionId).FirstOrDefaultAsync();
            var account = await _context.Accounts.Where(x => x.Id == transaction.AccountId).FirstOrDefaultAsync();

            if (dynamicCode.Password != model.Password || dynamicCode.ExpireAt < DateTime.Now)
            {
                transaction.Description = "رمز وارد شده اشتباه است";
                transaction.Status = false;

                return false;
            }
            else if (account.Amount < transaction.Amount)
            {
                transaction.Description = "موجودی کافی نیست";
                transaction.Status = false;

                return false;
            }

            transaction.Description = "انتقال پول با موفقیت انجام شد";
            transaction.Status = true;
            account.Amount -= transaction.Amount;

            var destinationAccount = await _context.Accounts.Where(x => x.CardNumber == transaction.DestinationCardNumber)
                                                     .FirstOrDefaultAsync();

            if (!(destinationAccount is null))
            {
                destinationAccount.Amount += transaction.Amount;
            }
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<TransactionReportDto>> GetTransactionsReport(String from, string to, bool isSuccess, int userId)
        {
            if (string.IsNullOrEmpty(from))
            {
                return await _context.Transactions.OrderByDescending(x => x.Id).Take(5)
                                                       .Select(x => new TransactionReportDto()
                                                       {
                                                           Amount = x.Amount,
                                                           Status = x.Status,
                                                           CreatedDateTime = x.CreatedDateTime,
                                                           AccountId = x.AccountId,
                                                           Description = x.Description,
                                                           DestinationCardNumber = x.DestinationCardNumber
                                                       }).ToListAsync();
            }
            return await _context.Transactions.Where(x => x.UserId == userId
                                                                && x.CreatedDateTime >= DateTime.Parse(from)
                                                                && x.CreatedDateTime <= DateTime.Parse(to)
                                                                && x.Status == isSuccess).Select(x => new TransactionReportDto()
                                                                {
                                                                    Amount = x.Amount,
                                                                    Status = x.Status,
                                                                    CreatedDateTime = x.CreatedDateTime,
                                                                    AccountId = x.AccountId,
                                                                    Description = x.Description,
                                                                    DestinationCardNumber = x.DestinationCardNumber
                                                                }).ToListAsync();
        }
    }
}