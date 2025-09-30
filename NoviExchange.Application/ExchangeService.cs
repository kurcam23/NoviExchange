using NoviExchange.Application.Interfaces.Providers;
using NoviExchange.Application.Interfaces.Services;
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
