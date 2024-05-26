using AccountsAPI.DbContexts;
using AccountsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountsAPI.Services
{
    public class UserService
    {
        private readonly LazureDbContext _dbContext;

        public UserService(LazureDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User AddNewUser(string walletId)    //Adding and saving the new user
        {
            var userToSave = new User
            {
                WalletId = walletId
            };

            try
            {
                _dbContext.Add(userToSave);
                _dbContext.SaveChanges();
                return userToSave;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                throw; 
            }
        }


        public User GetUserByWalletId(string walletId) //Getting a user by its wallet
        {
            return _dbContext.Users.FirstOrDefault(u => u.WalletId == walletId);
        }


        public string GetWalletIdByUser(int userId)  //Getting wallet by its user
        {
            User? user = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            return user.WalletId;
        }


        public string GetProductOwnerWalletByProductId(int productId)
        {
            Product? product = _dbContext.Products.FirstOrDefault(p => p.ProductId == productId);

            // Check if the product exists
            if (product == null)
            {
                throw new ArgumentException($"No product found with ID {productId}");
            }

            // Find the product owner by the user ID associated with the product
            User? user = _dbContext.Users.FirstOrDefault(u => u.UserId == product.UserId);

            // Check if the user exists
            if (user == null)
            {
                throw new ArgumentException($"No user found with ID {product.UserId}");
            }

            return user.WalletId;
        }

        public User GetUserByUserId(int userId)
        {
            return _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
        }
    }
}