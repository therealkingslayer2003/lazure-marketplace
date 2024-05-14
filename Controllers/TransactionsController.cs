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

        [HttpPost("add-new-completed-transaction")]
        public async Task<IActionResult> AddNewTransaction([FromBody] AddTransactionDto transactionDto)
        {

            try
            {
                Transaction savedTransaction = await transactionService.AddNewTransaction(transactionDto);
                return Ok(savedTransaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }

        [HttpGet("get-transaction/{transactionId}")]
        public ActionResult<Transaction> GetTransactionById(int transactionId)
        {  
            Transaction transaction = transactionService.GetTransactionById(transactionId);

            if (transaction == null) return NotFound("This transaction does not exist");

            return Ok(transaction);
        }
    }
}
