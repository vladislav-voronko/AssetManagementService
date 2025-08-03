using AssetManagementService.Domain.Aggregates.Asset;

namespace AssetManagementService.Domain.Interfaces
{
    public interface IAssetRepository
    {
        Task<Asset?> GetByIdAsync(Guid id);
        Task CreateAsync(Asset asset);
        Task DeleteAsync(Guid id);
    }
}
