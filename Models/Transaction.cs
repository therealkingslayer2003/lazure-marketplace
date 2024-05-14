using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AccountsAPI.DTOs;
using System.Text.Json.Serialization;

namespace AccountsAPI.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("transaction_id")]
        public int TransactionId { get; set; }

        [Column("seller_id")]
        [ForeignKey("SellerId")]
        public int SellerId { get; set; }

        [JsonIgnore]
        public virtual User Seller { get; set; }  // Navigation property for EF

        [Column("buyer_id")]
        [ForeignKey("BuyerId")]
        public int BuyerId { get; set; }

        [JsonIgnore]
        public virtual User Buyer { get; set; } // Navigation property for EF

        [Column("datetime")]
        public DateTime DateTime {  get; set; }

        [Column("product_id")]
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; } // Navigation property for EF

        [Column("txid")]
        public string? TxId { get; set; }

        public Transaction() { }

        public static Transaction CreateFromDto(AddTransactionDto transactionDto)   //Fabric method
        {
            if (transactionDto == null)
            {
                throw new ArgumentNullException(nameof(transactionDto), "Transaction DTO cannot be null");
            }

            return new Transaction
            {
                SellerId = transactionDto.SellerId,
                BuyerId = transactionDto.BuyerId,
                DateTime = transactionDto.DateTime,
                ProductId = transactionDto.ProductId,
                TxId = transactionDto.TxId
            };
        }


    }
}
