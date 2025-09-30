using NoviExchange.Domain.Interfaces;

namespace NoviExchange.Application.Interfaces.Factories
{
    public interface IWalletAdjustmentStrategyFactory
    {
        IWalletAdjustmentStrategy Create(string strategyName);
    }
}
