using NoviExchange.Application.Interfaces.Providers;
using NoviExchange.Application.Interfaces.Repositories;
using NoviExchange.Domain.Entities;
using Quartz;

namespace NoviExchange.Application.Jobs
{
    public class ExchangeRateUpdateJob : IJob
    {
        private readonly IEcbProvider _ecbProvider;
        private readonly ICurrencyRateRepository _currencyRateRepository;

        public ExchangeRateUpdateJob(IEcbProvider ecbProvider, ICurrencyRateRepository repository)
        {
            _ecbProvider = ecbProvider;
            _currencyRateRepository = repository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var rates = await _ecbProvider.GetLatestRatesAsync();

            if (!rates.Any()) 
                return;

            //TODO: add AutoMapper to implementation
            var ratesEntities = rates.Select(r => new CurrencyRateEntity
            {
                FromCurrency = "EUR", //TODO: deal with this hardcoding
                ToCurrency = r.Currency,
                Rate = r.Rate,
                Date = r.Date
            }).ToList();

            await _currencyRateRepository.UpsertRatesAsync(ratesEntities);
        }
    }
}
