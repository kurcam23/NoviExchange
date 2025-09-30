using NoviExchange.Domain.Entities;

namespace NoviExchange.Application.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        Task<WalletEntity?> GetByIdAsync(long walletId);
        Task AddAsync(WalletEntity wallet);
        Task UpdateAsync(WalletEntity wallet);
    }
}
