using System.ComponentModel.DataAnnotations;

namespace NoviExchange.Api.DTOs
{
    public class AdjustBalanceRequest
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a 3-letter ISO code.")]
        public string Currency { get; set; } = null!;

        [Required]
        public string Strategy { get; set; } = null!;
    }
}
