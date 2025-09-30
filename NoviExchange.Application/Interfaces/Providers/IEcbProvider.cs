using NoviExchange.Domain.Models;

namespace NoviExchange.Application.Interfaces.Providers
{
    public interface IEcbProvider
    {
        Task<IEnumerable<CurrencyRate>> GetLatestRatesAsync();
    }
}
