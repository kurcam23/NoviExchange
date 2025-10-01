using AutoMapper;
using NoviExchange.Application.Interfaces.Repositories;
using NoviExchange.Application.Interfaces.Services;
using NoviExchange.Domain.Entities;
using NoviExchange.Domain.Interfaces;
using NoviExchange.Domain.Models;

namespace NoviExchange.Application
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IMapper _mapper;

        public WalletService(IWalletRepository walletRepository, ICurrencyRateRepository currencyRateRepository, IMapper mapper)
        {
            _walletRepository = walletRepository;
            _currencyRateRepository = currencyRateRepository;
            _mapper = mapper;
        }

        public async Task<Wallet> CreateWalletAsync(string currency, decimal initialBalance = 0)
        {
            var wallet = new Wallet(0, currency, initialBalance);
            var walletEntity = _mapper.Map<WalletEntity>(wallet);

            await _walletRepository.AddAsync(walletEntity);
            return wallet;
        }

        public async Task<decimal> GetBalanceAsync(long walletId, string? convertToCurrency = null)
        {
            var walletEntity = await _walletRepository.GetByIdAsync(walletId);

            if (walletEntity == null) throw new KeyNotFoundException("Wallet not found");
            var wallet = _mapper.Map<Wallet>(walletEntity);

            if (convertToCurrency == null || wallet.Currency == convertToCurrency)
                return wallet.Balance;

            return await GetConvertedBalance(convertToCurrency, wallet.Currency, wallet.Balance);
        }

        private async Task<decimal> GetConvertedBalance(string toCurrency, string fromCurrency, decimal balance)
        {
            var currencyRates = await _currencyRateRepository.GetLatestCurrencyRates(); //TODO: make this into cache

            if (!currencyRates.TryGetValue(fromCurrency, out var fromRate))
                throw new KeyNotFoundException($"Currency rate for '{fromCurrency}' not found");

            if (!currencyRates.TryGetValue(toCurrency, out var toRate))
                throw new KeyNotFoundException($"Currency rate for '{toCurrency}' not found");

            return balance * (toRate / fromRate);
        }

        public async Task AdjustBalanceAsync(long walletId, decimal amount, string currency, IWalletAdjustmentStrategy strategy)
        {
            var walletEntity = await _walletRepository.GetByIdAsync(walletId);
            if (walletEntity == null) throw new KeyNotFoundException("Wallet not found");

            var wallet = _mapper.Map<Wallet>(walletEntity);
            var convertedAmount = await GetConvertedBalance(wallet.Currency, currency, amount);
            wallet.AdjustBalance(convertedAmount, strategy);

            _mapper.Map(wallet, walletEntity);
            await _walletRepository.UpdateAsync(walletEntity);
        }
    }
}
