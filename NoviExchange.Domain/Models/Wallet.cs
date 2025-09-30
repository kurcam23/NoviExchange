using NoviExchange.Domain.Interfaces;

namespace NoviExchange.Domain.Models
{
    public class Wallet
    {
        public long Id { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; } = null!;

        public Wallet(long id, string currency, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be empty", nameof(currency));

            Id = id;
            Currency = currency;
            Balance = initialBalance;
        }

        public void AdjustBalance(decimal amount, IWalletAdjustmentStrategy strategy)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            strategy.Adjust(this, amount);
        }
        internal void Add(decimal amount) => Balance += amount;
        internal void Subtract(decimal amount) => Balance -= amount;
    }
}
