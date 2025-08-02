using AssetManagementService.Domain.Aggregates.Asset;
using AssetManagementService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

public class AssetRepository : IAssetRepository
{
    private readonly AssetManagementDbContext _dbContext;

    public AssetRepository(AssetManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Asset?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Assets
            .Include(a => a.Trades)
            .Include(a => a.Replenishments)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Asset asset)
    {
        await _dbContext.Assets.AddAsync(asset);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Asset asset)
    {
        _dbContext.Assets.Update(asset);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var asset = GetByIdAsync(id).Result;
        if (asset != null)
        {
            asset.MarkAsDeleted();
            await _dbContext.SaveChangesAsync();
        }
    }
}
