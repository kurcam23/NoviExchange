using NoviExchange.Domain.Interfaces;
using NoviExchange.Domain.Models;

namespace NoviExchange.Application.Interfaces.Services
{
    public interface IWalletService
    {
        Task<Wallet> CreateWalletAsync(string currency, decimal initialBalance = 0);
        Task<decimal> GetBalanceAsync(long walletId, string? convertToCurrency = null);
        Task AdjustBalanceAsync(long walletId, decimal amount, string currency, IWalletAdjustmentStrategy strategy);
    }
}
