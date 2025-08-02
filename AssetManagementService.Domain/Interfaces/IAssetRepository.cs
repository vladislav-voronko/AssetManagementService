using AssetManagementService.Domain.Aggregates.Asset;

namespace AssetManagementService.Domain.Interfaces
{
    public interface IAssetRepository
    {
        Task<Asset?> GetByIdAsync(Guid id);
        Task AddAsync(Asset asset);
        Task UpdateAsync(Asset asset);
        Task DeleteAsync(Guid id);
    }
}
