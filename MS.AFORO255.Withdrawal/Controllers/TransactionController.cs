using Aforo255.Cross.Event.Src.Bus;
using Microsoft.AspNetCore.Mvc;
using MS.AFORO255.Withdrawal.DTOs;
using MS.AFORO255.Withdrawal.Messages.Commands;
using MS.AFORO255.Withdrawal.Services;
using System;

namespace MS.AFORO255.Withdrawal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IEventBus _bus;
        private readonly IAccountService _accountService;
        public TransactionController( ITransactionService transactionService, IEventBus bus, IAccountService accountService)
        {
            _transactionService = transactionService;
            _bus = bus;
            _accountService = accountService;
        }

        [HttpPost("Withdrawal")]
        public IActionResult Withdrawal([FromBody] TransactionRequest request)
        {
            Models.Transaction transaction = new Models.Transaction()
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                CreationDate = DateTime.Now.ToShortDateString(),
                Type = "Withdrawal"
            };
            transaction = _transactionService.Withdrawal(transaction);

            bool isProccess = _accountService.Execute(transaction);
            if (isProccess)
            {
                var withdrawalCreateCommand = new WithdrawalCreateCommand(
                   idTransaction: transaction.Id,
                   amount: transaction.Amount,
                   type: transaction.Type,
                   creationDate: transaction.CreationDate,
                   accountId: transaction.AccountId
                );
                _bus.SendCommand(withdrawalCreateCommand);

                var notificationWithdrawalCreateCommand = new NotificationWithdrawalCreateCommand(
                   idTransaction: transaction.Id,
                   amount: transaction.Amount,
                   type: transaction.Type,
                   messageBody: "Retiro enviado",
                   address: "j.maradiaga@hotmail.com",
                   accountId: transaction.AccountId
                );
                _bus.SendCommand(notificationWithdrawalCreateCommand);

                return Ok(transaction);
            }
            return BadRequest(new { status = "Failed" });
        }
    }
}
