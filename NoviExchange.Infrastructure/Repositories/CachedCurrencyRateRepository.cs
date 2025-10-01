using NoviExchange.Application.Interfaces.Repositories;
using NoviExchange.Application.Interfaces.Services;
using NoviExchange.Domain.Entities;

namespace NoviExchange.Infrastructure.Repositories
{
    public class CachedCurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly ICurrencyRateRepository _inner;
        private readonly ICacheService _cache;

        private const string CacheKey = "CurrencyRates";

        public CachedCurrencyRateRepository(ICurrencyRateRepository inner, ICacheService cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task<Dictionary<string, decimal>> GetLatestCurrencyRates()
        {
            var cachedRates = await _cache.GetAsync<Dictionary<string, decimal>>(CacheKey);
            if (cachedRates != null && cachedRates.Any())
                return cachedRates;

            var dbRates = await _inner.GetLatestCurrencyRates();
            await _cache.SetAsync(CacheKey, dbRates, TimeSpan.FromHours(1));

            return dbRates;
        }

        public async Task UpsertRatesAsync(IEnumerable<CurrencyRateEntity> rates)
        {
            await _inner.UpsertRatesAsync(rates);

            var currencyRates = rates.ToDictionary(r => r.ToCurrency, r => r.Rate);
            await _cache.SetAsync("CurrencyRates", currencyRates, TimeSpan.FromHours(1));
        }
    }
}
