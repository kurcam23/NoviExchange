using Microsoft.AspNetCore.Mvc;
using NoviExchange.Application.Interfaces.Factories;
using NoviExchange.Application.Interfaces.Services;

namespace NoviExchange.Api.Controllers
{
    [ApiController]
    [Route("api/wallets")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IWalletAdjustmentStrategyFactory _walletAdjustmentStrategyFactory;

        public WalletController(IWalletService walletService, IWalletAdjustmentStrategyFactory walletAdjustmentStrategyFactory)
        {
            _walletService = walletService;
            _walletAdjustmentStrategyFactory = walletAdjustmentStrategyFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] string currency)
        {
            var wallet = await _walletService.CreateWalletAsync(currency);
            return Ok(wallet);
        }

        [HttpGet("{walletId}")]
        public async Task<IActionResult> GetBalance(long walletId, [FromQuery] string? currency = null)
        {
            var balance = await _walletService.GetBalanceAsync(walletId, currency);
            return Ok(balance);
        }

        [HttpPost("{walletId}/adjustbalance")]
        public async Task<IActionResult> AdjustBalance(
            long walletId,
            [FromQuery] decimal amount,
            [FromQuery] string currency,
            [FromQuery] string strategy)
        {
            var strat = _walletAdjustmentStrategyFactory.Create(strategy);

            await _walletService.AdjustBalanceAsync(walletId, amount, currency, strat);
            return Ok();
        }
    }
}
