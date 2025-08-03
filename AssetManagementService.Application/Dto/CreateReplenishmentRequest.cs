namespace AssetManagementService.Application.Dto
{
    public class CreateReplenishmentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public DateTimeOffset Date { get; set; }
        public string? Note { get; set; }
    }
}
