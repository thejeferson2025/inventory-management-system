using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transactions.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        public string Type { get; set; } = string.Empty; 

        public Guid ProductId { get; set; } 

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public string? Detail { get; set; }
    }
}