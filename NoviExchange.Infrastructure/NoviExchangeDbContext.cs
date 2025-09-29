using Microsoft.EntityFrameworkCore;
using NoviExchange.Domain.Entities;

namespace NoviExchange.Infrastructure
{
    public class NoviExchangeDbContext : DbContext
    {
        public NoviExchangeDbContext(DbContextOptions<NoviExchangeDbContext> options)
            : base(options)
        {
        }

        public DbSet<CurrencyRateEntity> CurrencyRates { get; set; } = null!;
        public DbSet<WalletEntity> Wallets { get; set; } = null!;
    }
}
