using NoviExchange.Application.Interfaces.Factories;
using NoviExchange.Domain.Interfaces;

namespace NoviExchange.Application.Factories
{
    public class WalletAdjustmentStrategyFactory : IWalletAdjustmentStrategyFactory
    {
        public IWalletAdjustmentStrategy Create(string strategyName)
        {
            return strategyName switch
            {
                "AddFundsStrategy" => new AddFundsStrategy(),
                "SubtractFundsStrategy" => new SubtractFundsStrategy(),
                "ForceSubtractFundsStrategy" => new ForceSubtractFundsStrategy(),
                _ => throw new ArgumentException($"Invalid strategy: {strategyName}")
            };
        }
    }
}
