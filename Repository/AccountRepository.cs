using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Internet_Bank.Data;
using Internet_Bank.Model.Account;
using Internet_Bank.Service;
using Microsoft.EntityFrameworkCore;

namespace Internet_Bank.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly InternetBankContext _context;
        private readonly IRandomService _randomService;


        public AccountRepository(
            InternetBankContext context,
            IRandomService randomService)
        {
            _context = context;
            _randomService = randomService;

        }

        public async Task<Account> OpenAccount(int userId, OpenAccountDto model)
        {
            var account = new Account()
            {
                AccountType = model.AccountType,
                Amount = model.Amount,
                AccountNumber = _randomService.AccountNumberGenerator(userId,model),
                CardNumber = _randomService.CardNumberGenerator(),
                CVV2 = _randomService.Cvv2Generator(),
                ExpireDate = DateTime.Now.AddYears(5).ToString("yy/MM"),
                StaticPassword = _randomService.PasswordGenerator(),
                IsBlocked = false,
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
                account.IsBlocked = true;
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
                account.IsBlocked = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}