using NoviExchange.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NoviExchange.Domain.Entities
{
    public class WalletEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
