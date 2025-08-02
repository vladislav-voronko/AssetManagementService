using AssetManagementService.Domain.Common;
using AssetManagementService.Domain.ValueObjects;

namespace AssetManagementService.Domain.Aggregates.Asset
{
    public class Replenishment : Entity<Guid>
    {
        public Guid AssetId { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public Money Amount { get; private set; }
        public string? Note { get; private set; }

        protected Replenishment() { } // EF Core

        public Replenishment(Guid id, Guid assetId, Money amount, DateTimeOffset date, string? note = null)
        {
            Id = id;
            AssetId = assetId;
            Amount = amount ?? throw new ArgumentNullException(nameof(amount));
            Date = date;
            Note = note;
        }

        public void MarkAsDeleted() => IsDeleted = true;
    }
}
