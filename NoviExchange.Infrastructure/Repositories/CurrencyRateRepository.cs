using Microsoft.EntityFrameworkCore;
using NoviExchange.Application.Interfaces;
using NoviExchange.Domain.Entities;

namespace NoviExchange.Infrastructure.Repositories
{
    public class CurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly NoviExchangeDbContext _context;

        public CurrencyRateRepository(NoviExchangeDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task UpsertRatesAsync(IEnumerable<CurrencyRateEntity> rates)
        {
            if (!rates.Any()) return;

            var valuesList = string.Join(", ",
            rates.Select(r => $"('{r.FromCurrency}', '{r.ToCurrency}', {r.Rate}, '{r.Date:yyyy-MM-dd}')"));

            var mergeSql = $@"
                MERGE INTO CurrencyRates AS target
                USING (VALUES
                    {valuesList}
                ) AS source (FromCurrency, ToCurrency, Rate, RateDate)
                ON target.FromCurrency = source.FromCurrency 
                   AND target.ToCurrency = source.ToCurrency 
                   AND target.Date = source.RateDate
                WHEN MATCHED THEN
                    UPDATE SET Rate = source.Rate
                WHEN NOT MATCHED THEN
                    INSERT (FromCurrency, ToCurrency, Rate, Date, CreatedAt)
                    VALUES (source.FromCurrency, source.ToCurrency, source.Rate, source.RateDate, GETDATE());";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Database.ExecuteSqlRawAsync(mergeSql);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to upsert currency rates", ex);
            }
        }
    }
}
