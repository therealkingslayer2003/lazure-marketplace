using AccountsAPI.DbContexts;
using AccountsAPI.DTOs;
using AccountsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountsAPI.Services
{
    public class TransactionService
    {
        private readonly LazureDbContext dbContext;

        public TransactionService(LazureDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Transaction AddNewTransaction(TransactionDto transactionDto)
        {
            var seller = dbContext.Users.Find(transactionDto.SellerId);
            var buyer = dbContext.Users.Find(transactionDto.BuyerId);
            var product = dbContext.Products.Find(transactionDto.ProductId);

            if (seller == null || buyer == null || product == null)
            {
                throw new Exception("One or more entities not found");
            }

            Transaction transactionToSave = Transaction.CreateFromDto(transactionDto);

            try
            {
                dbContext.Add(transactionToSave);
                dbContext.SaveChanges();
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
            var transaction = dbContext.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);

            return transaction;
        }

        public List<Transaction> GetTransactionsByWalletId(string walletId)
        {
            var user = dbContext.Users.SingleOrDefault(u => u.WalletId.Equals(walletId));

            if (user == null)
            {
                return new List<Transaction>(); 
            }

            var transactions = dbContext.Transactions
                .Where(t => t.BuyerId == user.UserId || t.SellerId == user.UserId)
                .OrderByDescending(t => t.DateTime) //To always have a new transaction on top of the table
                .ToList();

            return transactions;
        }
        public List<Transaction> GetTransactionsByCurrentUserBuyer(int userId)
        {
            return dbContext.Transactions.Where(t => t.BuyerId == userId).ToList();
        }
    }
}
