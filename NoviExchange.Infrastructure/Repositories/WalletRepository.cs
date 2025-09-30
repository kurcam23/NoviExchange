using NoviExchange.Application.Interfaces.Repositories;
using NoviExchange.Domain.Entities;

namespace NoviExchange.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly NoviExchangeDbContext _context;

        public WalletRepository(NoviExchangeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WalletEntity wallet)
        {
            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task<WalletEntity?> GetByIdAsync(long walletId)
        {
            return await _context.Wallets.FindAsync(walletId);
        }

        public async Task UpdateAsync(WalletEntity wallet)
        {
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
        }
    }
}
