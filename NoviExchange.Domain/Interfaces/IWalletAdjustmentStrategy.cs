using NoviExchange.Domain.Models;

namespace NoviExchange.Domain.Interfaces
{
    public interface IWalletAdjustmentStrategy
    {
        void Adjust(Wallet wallet, decimal amount);
    }

    public class AddFundsStrategy : IWalletAdjustmentStrategy
    {
        public void Adjust(Wallet wallet, decimal amount) => wallet.Add(amount);
    }

    public class SubtractFundsStrategy : IWalletAdjustmentStrategy
    {
        public void Adjust(Wallet wallet, decimal amount)
        {
            if (wallet.Balance < amount)
                throw new InvalidOperationException("Insufficient funds");
            wallet.Subtract(amount);
        }
    }

    public class ForceSubtractFundsStrategy : IWalletAdjustmentStrategy
    {
        public void Adjust(Wallet wallet, decimal amount) => wallet.Subtract(amount);
    }
}
