using NoviExchange.Domain.Interfaces;
using NoviExchange.Domain.Models;

namespace NoviExchange.Application;

public class ExchangeService : IExchangeService
{
    private readonly IEcbProvider _ecbProvider;

    public ExchangeService(IEcbProvider ecbProvider)
    {
        _ecbProvider = ecbProvider;
    }

    public async Task<IEnumerable<CurrencyRate>> GetRatesAsync()
    {
        return await _ecbProvider.GetLatestRatesAsync();
    }
}
