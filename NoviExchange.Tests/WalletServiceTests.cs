using AutoMapper;
using Moq;
using NoviExchange.Application.Interfaces.Repositories;
using NoviExchange.Application.Services;
using NoviExchange.Domain.Entities;
using NoviExchange.Domain.Models;

namespace NoviExchange.Tests
{
    public class WalletServiceTests
    {
        private readonly Mock<IWalletRepository> _mockWalletRepo;
        private readonly Mock<ICurrencyRateRepository> _mockCurrencyRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly WalletService _service;

        public WalletServiceTests()
        {
            _mockWalletRepo = new Mock<IWalletRepository>();
            _mockCurrencyRepo = new Mock<ICurrencyRateRepository>();
            _mockMapper = new Mock<IMapper>();

            _service = new WalletService(_mockWalletRepo.Object, _mockCurrencyRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateWalletAsync_ShouldReturnWallet_WithCorrectBalance()
        {
            // Arrange
            var wallet = new Wallet(0, "EUR", 100);
            _mockMapper.Setup(m => m.Map<WalletEntity>(It.IsAny<Wallet>())).Returns(new WalletEntity());

            // Act
            var result = await _service.CreateWalletAsync("EUR", 100);

            // Assert
            Assert.Equal("EUR", result.Currency);
            Assert.Equal(100, result.Balance);
            _mockWalletRepo.Verify(r => r.AddAsync(It.IsAny<WalletEntity>()), Times.Once);
        }

        [Fact]
        public async Task GetBalanceAsync_ReturnsWalletBalance_WhenConvertToCurrencyIsNull()
        {
            // Arrange
            var walletEntity = new WalletEntity { Id = 1, Currency = "USD", Balance = 100 };
            var wallet = new Wallet(1, "USD", 100);

            _mockWalletRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(walletEntity);
            _mockMapper.Setup(m => m.Map<Wallet>(walletEntity)).Returns(wallet);

            // Act
            var result = await _service.GetBalanceAsync(1, null);

            // Assert
            Assert.Equal(100, result);
        }

        [Fact]
        public async Task GetBalanceAsync_Throws_WhenWalletNotFound()
        {
            // Arrange
            _mockWalletRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((WalletEntity)null!);

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetBalanceAsync(1));
        }

        [Fact]
        public async Task GetBalanceAsync_CallsConversion_WhenCurrencyDifferent()
        {
            // Arrange
            var walletEntity = new WalletEntity { Id = 1, Currency = "USD", Balance = 100 };
            var wallet = new Wallet(1, "USD", 100);

            _mockWalletRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(walletEntity);
            _mockMapper.Setup(m => m.Map<Wallet>(walletEntity)).Returns(wallet);

            _mockCurrencyRepo.Setup(r => r.GetLatestCurrencyRates()).ReturnsAsync(new Dictionary<string, decimal>
            {
                { "USD", 1 },
                { "EUR", 0.5m }
            });

            // Act
            var result = await _service.GetBalanceAsync(1, "EUR");

            // Assert
            Assert.Equal(50, result);
        }
    }
}
