using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class BuyOrder
    {
        public Guid BuyOrderID { get; set; }
        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(0, 1000000)]
        public uint? Quantity { get; set; }

        [Range(0, 1000000)]
        public double? Price { get; set; }
    }
}
