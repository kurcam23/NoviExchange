using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NoviExchange.EcbClient;
using NoviExchange.EcbClient.Options;
using System.Net;

namespace NoviExchange.Tests
{
    public class EcbProviderTests
    {
        /// <summary>
        /// Creates an instance of EcbProvider for testing purposes,
        /// with a mocked HttpMessageHandler and configurable retry options.
        /// </summary>
        private EcbProvider CreateClient(HttpMessageHandler handler, int maxRetries = 5, int initialDelayMs = 1000)
        {
            var httpClient = new HttpClient(handler);

            var options = Microsoft.Extensions.Options.Options.Create(
                new EcbClientOptions
                {
                    EcbUrl = "http://fakeurl",
                    MaxRetries = maxRetries,
                    InitialDelayMs = initialDelayMs
                }
            );

            var loggerMock = new Mock<ILogger<EcbProvider>>();

            return new EcbProvider(httpClient, options, loggerMock.Object);
        }

        /// <summary>
        /// Fetches the latest currency rates.
        /// Parses currency rates from the XML.
        /// </summary>
        /// <returns>A collection of CurrencyRate objects.</returns>
        [Fact]
        public async Task GetLatestRatesAsync_ParsesRatesCorrectly()
        {
            // Arrange
            var fakeXml = @"
            <gesmes:Envelope xmlns:gesmes='http://www.gesmes.org/xml/2002-08-01' xmlns='http://www.ecb.int/vocabulary/2002-08-01/eurofxref'>
                <Cube>
                    <Cube time='2025-09-29'>
                        <Cube currency='USD' rate='1.5'/>
                        <Cube currency='GBP' rate='0.5'/>
                        <Cube currency='FAKE' rate='test'/>
                        <Cube currency='' rate='1'/>
                    </Cube>
                </Cube>
            </gesmes:Envelope>";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.IsAny<HttpRequestMessage>(),
                   ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(fakeXml)
               });

            var client = CreateClient(handlerMock.Object);

            // Act
            var result = await client.GetLatestRatesAsync();

            // Assert
            Assert.NotNull(result);
            var rates = result.ToList();

            Assert.Contains(rates, r => r.Currency == "EUR" && r.Rate == 1m);
            Assert.Contains(rates, r => r.Currency == "USD" && r.Rate == 1.5m);
            Assert.Contains(rates, r => r.Currency == "GBP" && r.Rate == 0.5m);
            Assert.Equal(3, rates.Count);
        }

        /// <summary>
        /// Fetches the latest currency rates.
        /// Parses currency rates from the XML.
        /// Retries up to 3 times on HTTP failures.
        /// </summary>
        /// <returns>A collection of CurrencyRate objects.</returns>
        [Fact]
        public async Task GetLatestRatesAsync_RetriesOnHttpRequestException()
        {
            // Arrange
            var fakeXml = "<Cube><Cube currency='USD' rate='1.5'/></Cube>";

            int callCount = 0;

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() =>
                {
                    callCount++;
                    if (callCount < 3)
                        throw new HttpRequestException("Temporary failure");

                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(fakeXml)
                    };
                });

            var client = CreateClient(handlerMock.Object, maxRetries: 5, initialDelayMs: 1);

            // Act
            var result = await client.GetLatestRatesAsync();

            // Assert
            Assert.Contains(result, r => r.Currency == "USD" && r.Rate == 1.5m);
            Assert.Equal(3, callCount);
        }
    }
}
