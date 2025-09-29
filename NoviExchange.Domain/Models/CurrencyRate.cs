namespace NoviExchange.Domain.Models
{
    public class CurrencyRate
    {
        public string Currency { get; set; } = string.Empty;
        public decimal Rate { get; set; }

        public CurrencyRate() { }
        public CurrencyRate(string currency, decimal rate)
        {
            Currency = currency;
            Rate = rate;
        }
    }
}
