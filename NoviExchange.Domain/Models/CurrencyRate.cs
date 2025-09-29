namespace NoviExchange.Domain.Models
{
    public class CurrencyRate
    {
        public string Currency { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }

        public CurrencyRate() { }
        public CurrencyRate(string currency, decimal rate, DateTime date)
        {
            Currency = currency;
            Rate = rate;
            Date = date;
        }
    }
}
