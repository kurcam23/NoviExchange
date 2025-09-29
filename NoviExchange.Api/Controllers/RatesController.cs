using Microsoft.AspNetCore.Mvc;
using NoviExchange.Application.Interfaces;

namespace NoviExchange.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatesController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;

        public RatesController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        //Temporary get exchange rates controller method to retrieve all exchange rates
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rates = await _exchangeService.GetRatesAsync();
            return Ok(rates);
        }
    }
}
