using AccountsAPI.DTOs;
using AccountsAPI.Models;
using AccountsAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccountsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : Controller
    {
        private readonly TransactionService transactionService;

        public TransactionsController(TransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpPost()]
        public IActionResult AddNewTransaction([FromBody] AddTransactionDto transactionDto)
        {

            try
            {
                Transaction savedTransaction = transactionService.AddNewTransaction(transactionDto);
                return Ok();
            }
            catch (Exception ex)
            {
                var errorResponse = new Dictionary<string, string>
        {
            { "message", "An error occurred while processing your request: " + ex.Message }
        };
                return StatusCode(500, errorResponse);
            }
        }

        //[HttpGet("/{transactionId}")]
        //public ActionResult<Transaction> GetTransactionByTxId(string txId)
        //{  
        //    Transaction transaction = transactionService.GetTransactionByTxId(txId);

        //    if (transaction == null) return NotFound("This transaction does not exist");

        //    return Ok(transaction);
        //}

        [HttpGet()]
        public List<Transaction> GetTransactionsByWalletId([FromQuery] string walletId)
        {
            return transactionService.GetTransactionsByWalletId(walletId);
        }

        [HttpGet("my-purchases")]
        public ActionResult<List<Transaction>> GetTransactionsByCurrentUserBuyer()
        {  
            var userId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata")?.Value;

            if (userId == null)
            {
                var errorResponse = new Dictionary<string, string>
        {
            { "message", "JWT token is invalid or was not added to the header" }
        };
                return Unauthorized(errorResponse);
            }


            var transactions = transactionService.GetTransactionsByCurrentUserBuyer(Int32.Parse(userId));

            if (transactions == null)
            {
                var errorResponse = new Dictionary<string, string>
        {
            { "message", "No transactions found for the specified product" }
        };
                return NotFound(errorResponse);
            }

            return Ok(transactions);
        }
    }
}
