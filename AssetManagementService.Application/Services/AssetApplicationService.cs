using AssetManagementService.Domain.Aggregates.Asset;
using AssetManagementService.Domain.Aggregates.Asset.Enums;
using AssetManagementService.Domain.Interfaces;
using AssetManagementService.Domain.ValueObjects;

namespace AssetManagementService.Application.Services;

public class AssetApplicationService
{
    private readonly IAssetRepository _assetRepository;
    private readonly AssetManagementDbContext _dbContext;

    public AssetApplicationService(IAssetRepository assetRepository, AssetManagementDbContext dbContext)
    {
        _assetRepository = assetRepository;
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateAssetAsync(string symbol, string name)
    {
        var asset = new Asset(Guid.NewGuid(), symbol, name);
        await _assetRepository.CreateAsync(asset);
        await _dbContext.SaveChangesAsync();

        return asset.Id;
    }

    public async Task AddTradeAsync(Guid assetId, TradeType type, decimal amount, Money price, DateTimeOffset date, bool isReinvested = false)
    {
        var asset = await _assetRepository.GetByIdAsync(assetId)
                     ?? throw new InvalidOperationException("Asset not found");

        asset.AddTrade(type, amount, price, date, isReinvested);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddReplenishmentAsync(Guid assetId, Money amount, DateTimeOffset date, string? note = null)
    {
        var asset = await _assetRepository.GetByIdAsync(assetId)
                     ?? throw new InvalidOperationException("Asset not found");

        asset.AddReplenishment(amount, date, note);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAssetAsync(Guid assetId)
    {
        await _assetRepository.DeleteAsync(assetId);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveTradeAsync(Guid assetId, Guid tradeId)
    {
        var asset = await _assetRepository.GetByIdAsync(assetId)
                     ?? throw new InvalidOperationException("Asset not found");

        asset.RemoveTrade(tradeId);

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveReplenishmentAsync(Guid assetId, Guid replenishmentId)
    {
        var asset = await _assetRepository.GetByIdAsync(assetId)
                     ?? throw new InvalidOperationException("Asset not found");

        asset.RemoveReplenishment(replenishmentId);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<decimal> GetTotalHoldingsAsync(Guid assetId)
    {
        var asset = await _assetRepository.GetByIdAsync(assetId)
                     ?? throw new InvalidOperationException("Asset not found");

        return asset.GetTotalHoldings();
    }

    public async Task<decimal> GetAverageBuyPriceAsync(Guid assetId)
    {
        var asset = await _assetRepository.GetByIdAsync(assetId)
                     ?? throw new InvalidOperationException("Asset not found");

        return asset.CalculateAverageBuyPrice();
    }
}
