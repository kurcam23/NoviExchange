using NoviExchange.Domain.Interfaces;
using NoviExchange.Domain.Models;

namespace NoviExchange.Tests
{
    public class WalletStrategyTests
    {
        [Fact]
        public void AddFundsStrategy_IncreasesBalance()
        {
            // Arrange
            var wallet = new Wallet(1, "USD", 100);
            var strategy = new AddFundsStrategy();

            // Act
            wallet.AdjustBalance(50, strategy);

            // Assert
            Assert.Equal(150, wallet.Balance);
        }

        [Fact]
        public void SubtractFundsStrategy_DecreasesBalance()
        {
            // Arrange
            var wallet = new Wallet(1, "USD", 100);
            var strategy = new SubtractFundsStrategy();

            // Act
            wallet.AdjustBalance(40, strategy);

            // Assert
            Assert.Equal(60, wallet.Balance);
        }

        [Fact]
        public void SubtractFundsStrategy_Throws_WhenInsufficientFunds()
        {
            // Arrange
            var wallet = new Wallet(1, "USD", 30);
            var strategy = new SubtractFundsStrategy();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => wallet.AdjustBalance(50, strategy));
        }

        [Fact]
        public void ForceSubtractFundsStrategy_AllowsNegativeBalance()
        {
            // Arrange
            var wallet = new Wallet(1, "USD", 30);
            var strategy = new ForceSubtractFundsStrategy();

            // Act
            wallet.AdjustBalance(50, strategy);

            // Assert
            Assert.Equal(-20, wallet.Balance);
        }
    }
}
