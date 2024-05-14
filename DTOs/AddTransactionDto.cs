using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AccountsAPI.DTOs
{
    public class AddTransactionDto
    {
        [Required]
        public int SellerId { get; set; }
        [Required]
        public int BuyerId { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string TxId { get; set; }
    }
}
