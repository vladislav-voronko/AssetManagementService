using AssetManagementService.Application.Dto;
using AssetManagementService.Application.Services;
using AssetManagementService.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AssetsController : ControllerBase
{
    private readonly AssetApplicationService _assetService;

    public AssetsController(AssetApplicationService assetService)
    {
        _assetService = assetService;
    }

    private IActionResult HandleNotFound(Func<Task> action, string notFoundMessage)
    {
        try
        {
            action().Wait();
            return NoContent();
        }
        catch (AggregateException ae) when (ae.InnerException is InvalidOperationException)
        {
            return NotFound(notFoundMessage);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("{assetId:guid}/trades")]
    public Task<IActionResult> AddTrade(Guid assetId, [FromBody] CreateTradeRequest request)
    {
        var price = new Money(request.PriceAmount, request.PriceCurrency);
        return Task.FromResult(HandleNotFound(
            () => _assetService.AddTradeAsync(assetId, request.Type, request.Amount, price, request.Date, request.IsReinvested),
            $"Asset with id {assetId} not found"));
    }

    [HttpDelete("{assetId:guid}/trades/{tradeId:guid}")]
    public Task<IActionResult> RemoveTrade(Guid assetId, Guid tradeId)
    {
        return Task.FromResult(HandleNotFound(
            () => _assetService.RemoveTradeAsync(assetId, tradeId),
            $"Asset with id {assetId} not found"));
    }

    [HttpPost("{assetId:guid}/replenishments")]
    public Task<IActionResult> AddReplenishment(Guid assetId, [FromBody] CreateReplenishmentRequest request)
    {
        var amount = new Money(request.Amount, request.Currency);
        return Task.FromResult(HandleNotFound(
            () => _assetService.AddReplenishmentAsync(assetId, amount, request.Date, request.Note),
            $"Asset with id {assetId} not found"));
    }

    [HttpDelete("{assetId:guid}/replenishments/{replenishmentId:guid}")]
    public Task<IActionResult> RemoveReplenishment(Guid assetId, Guid replenishmentId)
    {
        return Task.FromResult(HandleNotFound(
            () => _assetService.RemoveReplenishmentAsync(assetId, replenishmentId),
            $"Asset with id {assetId} not found"));
    }

    [HttpDelete("{assetId:guid}")]
    public Task<IActionResult> DeleteAsset(Guid assetId)
    {
        return Task.FromResult(HandleNotFound(
            () => _assetService.DeleteAssetAsync(assetId),
            $"Asset with id {assetId} not found"));
    }

    [HttpGet("{assetId:guid}/holdings")]
    public async Task<IActionResult> GetTotalHoldings(Guid assetId)
    {
        try
        {
            var total = await _assetService.GetTotalHoldingsAsync(assetId);
            return Ok(total);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Asset with id {assetId} not found");
        }
    }

    [HttpGet("{assetId:guid}/average-buy-price")]
    public async Task<IActionResult> GetAverageBuyPrice(Guid assetId)
    {
        try
        {
            var avgPrice = await _assetService.GetAverageBuyPriceAsync(assetId);
            return Ok(avgPrice);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Asset with id {assetId} not found");
        }
    }
}
