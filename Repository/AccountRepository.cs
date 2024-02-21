using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Internet_Bank.Data;
using Internet_Bank.Model.Account;
using Microsoft.EntityFrameworkCore;

namespace Internet_Bank.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly InternetBankContext _context;

        public AccountRepository(InternetBankContext context)
        {
            _context = context;
        }

        public async Task<Account> OpenAccount(int userId, OpenAccountDto model)
        {
            // Account Number
            Random random = new Random();
            int randomNumber1 = random.Next(10, 100);
            int randomNumber2 = random.Next(1000, 10000);

            var newuserId = "";
            if (userId < 10)
            {
                newuserId = "00" + userId.ToString();
            }
            else if (userId < 100)
            {
                newuserId = "0" + userId.ToString();
            }
            else
            {
                newuserId = userId.ToString();
            }

            var endDigit = (int)model.AccountType;

            int randomNumber3 = random.Next(1000, 10000);
            int randomNumber4 = random.Next(1000, 10000);
            int randomNumber5 = random.Next(1000, 10000);
            int randomNumber6 = random.Next(1000, 10000);

            // cvv2
            int randomNumber7 = random.Next(1000, 10000);

            // static password
            int randomNumber8 = random.Next(100000, 1000000);

            // var expire = DateTime.Now.AddYears(5).ToShortPersianDateString();
            // var expireYear = DateTime.Now.AddYears(5).Year;
            // var expireMonth = DateTime.Now.AddYears(5).Month;

            var account = new Account()
            {
                AccountType = model.AccountType,
                Amount = model.Amount,
                AccountNumber = randomNumber1.ToString() + "." + newuserId + randomNumber2.ToString() + "." + endDigit.ToString(),
                CardNumber = randomNumber3.ToString() + " " + randomNumber4.ToString() + " " + randomNumber5.ToString() + " " + randomNumber6.ToString() + " ",
                CVV2 = randomNumber7.ToString(),
                ExpireDate = DateTime.Now.AddYears(5),
                StaticPassword = randomNumber8.ToString(),
                Block = false,
                UserId = userId
            };
            _context.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<bool> ChangePassword(ChangePasswordDto model)
        {
            var account = await _context.Accounts.Where(x => x.AccountId == model.AccountId).FirstOrDefaultAsync();
            if (account != null)
            {
                if (account.StaticPassword == model.OldPassword)
                {
                    account.StaticPassword = model.NewPassword;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }

            return false;
        }

        public async Task<List<GetAccountsListsDto>> GetAccountsOfUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                var accounts = _context.Accounts.Where(x => x.UserId == user.Id).ToList();
                var accountsList = new List<GetAccountsListsDto>();
                foreach (var item in accounts)
                {
                    var account = new GetAccountsListsDto()
                    {
                        AccountNumber = item.AccountNumber,
                        AccountId = item.AccountId,
                        CardNumber = item.CardNumber
                    };
                    accountsList.Add(account);
                }
                return accountsList;
            }
            return new List<GetAccountsListsDto>();
        }

        public async Task<GetDetailsOfAccount> GetDetailsById(int account_id)
        {
            var account = await _context.Accounts.Where(x => x.AccountId == account_id)
                                            .Select(x => new GetDetailsOfAccount()
                                            {
                                                AccountNumber = x.AccountNumber,
                                                CardNumber = x.CardNumber,
                                                CVV2 = x.CVV2,
                                                ExpireDate = x.ExpireDate,
                                                StaticPassword = x.StaticPassword,
                                                AccountId = x.AccountId,
                                                AccountType = x.AccountType
                                            }).FirstOrDefaultAsync();
            return account;
        }

        public async Task<GetBalanceDto> GetBalanceById(int account_id)
        {
            var account = await _context.Accounts.Where(x => x.AccountId == account_id)
                                                .Select(x => new GetBalanceDto()
                                                {
                                                    AccountId = x.AccountId,
                                                    Amount = x.Amount,
                                                    AccountNumber = x.AccountNumber
                                                }).FirstOrDefaultAsync();
            return account;

        }

        public async Task<bool> BlockAccount(int account_id)
        {
            var account = await _context.Accounts.Where(x => x.AccountId == account_id).FirstOrDefaultAsync();

            if (account != null)
            {
                account.Block = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UnBlockAccount(int account_id)
        {
            var account = await _context.Accounts.Where(x => x.AccountId == account_id).FirstOrDefaultAsync();

            if (account != null)
            {
                account.Block = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}