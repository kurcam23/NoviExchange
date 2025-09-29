using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoviExchange.Domain.Interfaces;
using NoviExchange.Domain.Models;
using NoviExchange.EcbClient.Options;
using System.Globalization;
using System.Xml.Linq;

namespace NoviExchange.EcbClient;

public class EcbProvider : IEcbProvider
{
    private readonly HttpClient _httpClient;
    private readonly EcbClientOptions _options;
    private readonly ILogger<EcbProvider> _logger;

    public EcbProvider(HttpClient httpClient, IOptions<EcbClientOptions> options, ILogger<EcbProvider> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<IEnumerable<CurrencyRate>> GetLatestRatesAsync()
    {
        try
        {
            var xmlString = await FetchEcbXmlWithRetryAsync();
            var doc = XDocument.Parse(xmlString);

            var rates = new List<CurrencyRate> { new("EUR", 1m) };

            var ecbRates = doc.Descendants()
                            .Where(x => x.Name.LocalName == "Cube")
                            .Select(x => ParseCubeNode(x))
                            .OfType<CurrencyRate>()
                            .ToList();

            rates.AddRange(ecbRates);

            return rates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch or parse ECB rates");
            throw;
        }
    }

    private async Task<string> FetchEcbXmlWithRetryAsync()
    {
        var delay = _options.InitialDelayMs;

        for (int attempt = 1; attempt <= _options.MaxRetries; attempt++)
        {
            try
            {
                using var response = await _httpClient.GetAsync(_options.EcbUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Attempt {Attempt} to fetch ECB XML failed", attempt);

                if (attempt == _options.MaxRetries)
                {
                    _logger.LogError("All attempts to fetch ECB XML have failed");
                    throw;
                }

                await Task.Delay(delay);
                delay *= 2;
            }
        }

        return string.Empty;
    }

    private CurrencyRate? ParseCubeNode(XElement element)
    {
        var currencyAttr = element.Attribute("currency")?.Value;
        var rateAttr = element.Attribute("rate")?.Value;

        if (string.IsNullOrWhiteSpace(currencyAttr) || string.IsNullOrWhiteSpace(rateAttr))
        {
            _logger.LogWarning("Missing attribute in Cube node. Node: {CubeXml}", element);
            return null;
        }

        if (!decimal.TryParse(rateAttr, NumberStyles.Any, CultureInfo.InvariantCulture, out var rate))
        {
            _logger.LogWarning("Invalid rate in Cube node. Currency: {Currency}, Rate: {RateValue}", currencyAttr, rateAttr);
            return null;
        }

        return new CurrencyRate(currencyAttr, rate);
    }
}
