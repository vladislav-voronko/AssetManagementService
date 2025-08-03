using AssetManagementService.Domain.Aggregates.Asset.Enums;
using AssetManagementService.Domain.Common;
using AssetManagementService.Domain.ValueObjects;

namespace AssetManagementService.Domain.Aggregates.Asset
{
    public class Asset : AggregateRoot<Guid>
    {
        public string Symbol { get; private set; }
        public string Name { get; private set; }

        private readonly List<Trade> _trades = new();
        public IReadOnlyCollection<Trade> Trades => _trades.AsReadOnly();

        private readonly List<Replenishment> _replenishments = new();
        public IReadOnlyCollection<Replenishment> Replenishments => _replenishments.AsReadOnly();

        protected Asset() { } // Для EF Core

        public Asset(Guid id, string symbol, string name)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbol is required.", nameof(symbol));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            Id = id;
            Symbol = symbol;
            Name = name;
        }

        public void AddTrade(TradeType type, decimal amount, Money price, DateTimeOffset date, bool isReinvested = false)
        {
            if (price == null)
                throw new ArgumentNullException(nameof(price));

            if (price.Amount <= 0)
                throw new ArgumentException("Price must be positive.", nameof(price));

            var trade = new Trade(Guid.NewGuid(), Id, type, amount, price, date, isReinvested);
            _trades.Add(trade);
        }

        public void AddReplenishment(Money amount, DateTimeOffset date, string? note = null)
        {
            if (amount == null)
                throw new ArgumentNullException(nameof(amount));

            if (amount.Amount <= 0)
                throw new ArgumentException("Amount must be positive.", nameof(amount));

            var replenishment = new Replenishment(Guid.NewGuid(), Id, amount, date, note);
            _replenishments.Add(replenishment);
        }

        public void RemoveTrade(Guid tradeId)
        {
            var trade = _trades.FirstOrDefault(t => t.Id == tradeId);
            if (trade != null)
            {
                trade.MarkAsDeleted();
            }
        }

        public void RemoveReplenishment(Guid replenishmentId)
        {
            var replenishment = _replenishments.FirstOrDefault(r => r.Id == replenishmentId);
            if (replenishment != null)
            {
                replenishment.MarkAsDeleted();
            }
        }

        public decimal GetTotalHoldings()
        {
            var bought = _trades.Where(t => t.Type == TradeType.Buy).Sum(t => t.Amount);
            var sold = _trades.Where(t => t.Type == TradeType.Sell).Sum(t => t.Amount);
            return bought - sold;
        }

        public decimal CalculateAverageBuyPrice()
        {
            var buyTrades = _trades.Where(t => t.Type == TradeType.Buy).ToList();
            if (!buyTrades.Any()) return 0;

            decimal totalAmount = buyTrades.Sum(t => t.Amount);
            // t.Total — Money, у которого есть метод Multiply и Value. Надо получить decimal
            // Предполагаю, что Money имеет decimal Value свойство.
            decimal totalSpent = buyTrades.Sum(t => t.Total.Amount);

            return totalAmount == 0 ? 0 : totalSpent / totalAmount;
        }
        public void MarkAsDeleted()
        {
            IsDeleted = true;

            foreach (var trade in _trades)
                trade.MarkAsDeleted();

            foreach (var replenishment in _replenishments)
                replenishment.MarkAsDeleted();
        }

        public bool IsActive => !IsDeleted;
    }
}
