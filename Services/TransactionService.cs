using AccountsAPI.DbContexts;
using AccountsAPI.DTOs;
using AccountsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountsAPI.Services
{
    public class TransactionService
    {
        private readonly LazureDbContext _dbContext;

        public TransactionService(LazureDbContext dbContext)
        {
            _dbContext = dbContext;            
        }

        public async Task<Transaction> AddNewTransaction(AddTransactionDto transactionDto)
        {
            var seller = await _dbContext.Users.FindAsync(transactionDto.SellerId);
            var buyer = await _dbContext.Users.FindAsync(transactionDto.BuyerId);
            var product = await _dbContext.Products.FindAsync(transactionDto.ProductId);

            if (seller == null || buyer == null || product == null)
            {
                throw new Exception("One or more entities not found");
            }

            Transaction transactionToSave = Transaction.CreateFromDto(transactionDto);

            try
            {
                await _dbContext.AddAsync(transactionToSave);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("A concurrency error happened.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred.", ex);
            }
            return transactionToSave;
        }

        public Transaction GetTransactionById(int transactionId)
        {
            var transaction = _dbContext.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);

            return transaction;
        }
    }
}
