using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Internet_Bank.Data;
using Internet_Bank.Model.Account;
using Internet_Bank.Model.User;
using Internet_Bank.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace Internet_Bank.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]

    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountsRepository;
        private readonly ILogger<AccountController> _logger;


        public AccountController(
            IAccountRepository accountsRepository,
            ILogger<AccountController> logger)
        {
            _accountsRepository = accountsRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> OpenAccount([FromBody] OpenAccountDto model)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString != null)
            {
                var userId = int.Parse(userIdString);
                var account = await _accountsRepository.OpenAccount(userId, model);
                return Ok(account);
            }
            else
            {
                return Unauthorized("The user not found");
            }
        }


        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var result = await _accountsRepository.ChangePassword(model);
            if (!result)
            {
                return BadRequest("This account is not exist");
            }
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAccountsOfUser()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userIdString);

            var accounts = await _accountsRepository.GetAccountsOfUser(userId);
            if (accounts == null)
            {
                return NotFound();
            }
            return Ok(accounts);
        }


        [HttpGet("{account_id}")]
        public async Task<IActionResult> GetDetailsById(int account_id)
        {
            var account = await _accountsRepository.GetDetailsById(account_id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }


        [HttpGet("balance/{account_id}")]
        public async Task<IActionResult> GetBalanceById(int account_id)
        {
            var account = await _accountsRepository.GetBalanceById(account_id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpPut("block/{account_id}")]
        public async Task<IActionResult> BlockAccount(int account_id)
        {
            var result = await _accountsRepository.BlockAccount(account_id);
            if (!result)
            {
                return BadRequest("This account is not exist");
            }
            return Ok("The account is blocked");
        }

        [HttpPut("unblock/{account_id}")]
        public async Task<IActionResult> UnBlockAccount(int account_id)
        {
            var result = await _accountsRepository.UnBlockAccount(account_id);
            if (!result)
            {
                return BadRequest("This account is not exist");
            }
            return Ok("The account is unblocked");
        }


    }
}
