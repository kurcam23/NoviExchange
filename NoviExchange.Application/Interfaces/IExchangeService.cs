using NoviExchange.Domain.Models;

namespace NoviExchange.Application.Interfaces
{
    public interface IExchangeService
    {
        Task<IEnumerable<CurrencyRate>> GetRatesAsync();
    }
}
