using AssetManagementService.Domain.Aggregates.Asset.Enums;
using AssetManagementService.Domain.Common;
using AssetManagementService.Domain.ValueObjects;

namespace AssetManagementService.Domain.Aggregates.Asset
{
    public class Trade : Entity<Guid>
    {
        public Guid AssetId { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public TradeType Type { get; private set; }
        public decimal Amount { get; private set; }
        public Money Price { get; private set; }
        public bool IsReinvested { get; private set; }

        public Money Total => Price.Multiply(Amount);

        protected Trade() { } // EF Core

        public Trade(Guid id, Guid assetId, TradeType type, decimal amount, Money price, DateTimeOffset date, bool isReinvested)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            Id = id;
            AssetId = assetId;
            Type = type;
            Amount = amount;
            Price = price ?? throw new ArgumentNullException(nameof(price));
            Date = date;
            IsReinvested = isReinvested;
        }

        public void MarkAsDeleted() => IsDeleted = true;
    }
}
