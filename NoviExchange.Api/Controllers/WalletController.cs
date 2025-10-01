using Microsoft.AspNetCore.Mvc;
using NoviExchange.Api.DTOs;
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
        private readonly ILogger<WalletController> _logger;

        public WalletController(IWalletService walletService, IWalletAdjustmentStrategyFactory walletAdjustmentStrategyFactory, ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _walletAdjustmentStrategyFactory = walletAdjustmentStrategyFactory;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] CreateWalletRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var wallet = await _walletService.CreateWalletAsync(request.Currency);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating wallet for currency: {Currency}", request.Currency);
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{walletId}")]
        public async Task<IActionResult> GetBalance(long walletId, [FromQuery] GetBalanceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var balance = await _walletService.GetBalanceAsync(walletId, request.Currency);
                return Ok(balance);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Wallet not found: {WalletId}", walletId);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving balance for wallet {WalletId}", walletId);
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPost("{walletId}/adjustbalance")]
        public async Task<IActionResult> AdjustBalance(
            long walletId,
            [FromQuery] AdjustBalanceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var strat = _walletAdjustmentStrategyFactory.Create(request.Strategy);

                await _walletService.AdjustBalanceAsync(walletId, request.Amount, request.Currency, strat);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input for adjusting wallet {WalletId}", walletId);
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Wallet not found: {WalletId}", walletId);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error adjusting wallet {WalletId}", walletId);
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
