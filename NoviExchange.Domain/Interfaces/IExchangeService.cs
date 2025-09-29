using NoviExchange.Domain.Models;

namespace NoviExchange.Domain.Interfaces
{
    public interface IExchangeService
    {
        Task<IEnumerable<CurrencyRate>> GetRatesAsync();
    }
}
