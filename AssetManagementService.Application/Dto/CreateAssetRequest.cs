namespace AssetManagementService.Application.Dto
{
    public class CreateAssetRequest
    {
        public string Symbol { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
