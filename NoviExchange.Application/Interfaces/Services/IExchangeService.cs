using NoviExchange.Domain.Models;

namespace NoviExchange.Application.Interfaces.Services
{
    public interface IExchangeService
    {
        Task<IEnumerable<CurrencyRate>> GetRatesAsync();
    }
}
