using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoviExchange.Domain.Entities
{
    public class CurrencyRateEntity
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(3)]
        public string FromCurrency { get; set; } = null!;
        [Required]
        [StringLength(3)]
        public string ToCurrency { get; set; } = null!;
        [Required]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Rate { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
