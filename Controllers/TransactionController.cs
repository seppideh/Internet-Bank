using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters;
using Internet_Bank.Model.Transaction;
using Internet_Bank.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Internet_Bank.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(
            ITransactionRepository transactionRepository,
            ILogger<TransactionController> logger)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendSMS([FromBody] SendOtpDto model)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _transactionRepository.SendSMS(userId, model);

            if (result.Password != null)
            {
                return Ok("رمز پویا ارسال شد");
            }

            return BadRequest("کارت مورد نظر یافت نشد");
        }

        [HttpPatch("transfer-money")]
        public async Task<IActionResult> TransferMoney([FromBody] TransferMoneyDto model)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _transactionRepository.TransferMoney(userId, model);

            if (!result)
            {
                return BadRequest("انتقال پول ناموفق");
            }
            return Ok("انتقال پول با موفقیت انجام شد");
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetTransactionsReport(String from, string to, bool isSuccess)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _transactionRepository.GetTransactionsReport(from, to, isSuccess, userId);

            if (result.Count!=0)
            {
                return Ok(result);
            }
            return Ok("در بازه ی زمانی انتخاب شده تراکنشی وجود ندارد");
        }

    }
}