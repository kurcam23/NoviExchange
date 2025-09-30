using AutoMapper;
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
        private readonly IMapper _mapper;

        public ExchangeRateUpdateJob(IEcbProvider ecbProvider, ICurrencyRateRepository repository, IMapper mapper)
        {
            _ecbProvider = ecbProvider;
            _currencyRateRepository = repository; 
            _mapper = mapper;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var rates = await _ecbProvider.GetLatestRatesAsync();

            if (!rates.Any()) 
                return;

            var ratesEntities = _mapper.Map<List<CurrencyRateEntity>>(rates);


            await _currencyRateRepository.UpsertRatesAsync(ratesEntities);
        }
    }
}
