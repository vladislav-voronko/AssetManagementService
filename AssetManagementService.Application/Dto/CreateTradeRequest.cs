using AssetManagementService.Domain.Aggregates.Asset.Enums;

namespace AssetManagementService.Application.Dto
{
    public class CreateTradeRequest
    {
        public TradeType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal PriceAmount { get; set; }
        public string PriceCurrency { get; set; } = null!;
        public DateTimeOffset Date { get; set; }
        public bool IsReinvested { get; set; } = false;
    }
}
