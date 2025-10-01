using System.ComponentModel.DataAnnotations;

namespace NoviExchange.Api.DTOs
{
    public class GetBalanceRequest
    {
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a 3-letter ISO code.")]
        public string Currency { get; set; } = string.Empty;
    }
}
