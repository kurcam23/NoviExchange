using NoviExchange.Domain.Models;

namespace NoviExchange.Application.Interfaces
{
    public interface IEcbProvider
    {
        Task<IEnumerable<CurrencyRate>> GetLatestRatesAsync();
    }
}
