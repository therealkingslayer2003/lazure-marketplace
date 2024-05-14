using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsAPI.Models
{
    public class Product
    {
        [Key]
        [Column("product_id")]
        public int ProductId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("price")]
        public double Price { get; set; }
        [Column("user_id")]
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public virtual User User { get; set; } // Navigation property for EF

        [Column("resource_link")]
        public string ResourceLink { get; set; }
        [Column("category_id")]
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } // Navigation property for EF
        public virtual ICollection<Transaction> Transactions { get; set; }


    }
}
