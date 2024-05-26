using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AccountsAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("wallet_id")]
        public string WalletId { get; set; }
        [Column("username")]
        public string? Username { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
        [JsonIgnore]
        public virtual ICollection<Transaction> Sales { get; set; } // Транзакции, где пользователь является продавцом
        [JsonIgnore]    
        public virtual ICollection<Transaction> Purchases { get; set; } // Транзакции, где пользователь является покупателем

    } //LAZ-57
}
