using AccountsAPI.DTOs;
using AccountsAPI.Models;
using AccountsAPI.Services;
using Microsoft.AspNetCore.Mvc;

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
                return Ok(savedTransaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
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
    }
}
