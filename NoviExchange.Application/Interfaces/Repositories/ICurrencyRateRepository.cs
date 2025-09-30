using NoviExchange.Domain.Entities;
using NoviExchange.Domain.Models;

namespace NoviExchange.Application.Interfaces.Repositories
{
    public interface ICurrencyRateRepository
    {
        Task<Dictionary<string, decimal>> GetCurrencyRates();
        Task UpsertRatesAsync(IEnumerable<CurrencyRateEntity> rates);
    }
}
