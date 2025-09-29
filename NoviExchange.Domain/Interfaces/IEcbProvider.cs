using NoviExchange.Domain.Models;

namespace NoviExchange.Domain.Interfaces
{
    public interface IEcbProvider
    {
        Task<IEnumerable<CurrencyRate>> GetLatestRatesAsync();
    }
}
