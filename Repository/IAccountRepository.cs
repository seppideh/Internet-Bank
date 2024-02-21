using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internet_Bank.Data;
using Internet_Bank.Model.Account;

namespace Internet_Bank.Repository
{
    public interface IAccountRepository
    {
        Task<Account> OpenAccount(int userId, OpenAccountDto model);
        Task<bool> ChangePassword(ChangePasswordDto model);
        Task<List<GetAccountsListsDto>> GetAccountsOfUser(int userId);
        Task<GetDetailsOfAccount> GetDetailsById(int account_id);
        Task<GetBalanceDto> GetBalanceById(int account_id);
        Task<bool> BlockAccount(int account_id);
        Task<bool> UnBlockAccount(int account_id);
    }
}