using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userIdString);

            var result = await _transactionRepository.SendSMS(userId, model);

            if (result.Password != null)
            {
                return Ok("رمز پویا ارسال شد");
            }

            return BadRequest("کارت مورد نظر یافت نشد");
        }




    }
}