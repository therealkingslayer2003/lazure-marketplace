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

        public Transaction AddNewTransaction(AddTransactionDto transactionDto)
        {
            var seller = _dbContext.Users.Find(transactionDto.SellerId);
            var buyer = _dbContext.Users.Find(transactionDto.BuyerId);
            var product = _dbContext.Products.Find(transactionDto.ProductId);

            if (seller == null || buyer == null || product == null)
            {
                throw new Exception("One or more entities not found");
            }

            Transaction transactionToSave = Transaction.CreateFromDto(transactionDto);

            try
            {
                _dbContext.Add(transactionToSave);
                _dbContext.SaveChanges();
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

        public List<Transaction> GetTransactionsByWalletId(string walletId)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.WalletId.Equals(walletId));

            if (user == null)
            {
                return new List<Transaction>(); 
            }

            var transactions = _dbContext.Transactions
                .Where(t => t.BuyerId == user.UserId || t.SellerId == user.UserId)
                .ToList();

            return transactions;
        }
    }
}
