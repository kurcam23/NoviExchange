using NoviExchange.Domain.Entities;
using NoviExchange.Domain.Models;

namespace NoviExchange.Application.Interfaces
{
    public interface ICurrencyRateRepository
    {
        Task UpsertRatesAsync(IEnumerable<CurrencyRateEntity> rates);
    }
}
