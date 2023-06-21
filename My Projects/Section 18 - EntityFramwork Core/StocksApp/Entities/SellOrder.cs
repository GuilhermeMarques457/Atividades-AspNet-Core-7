using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class SellOrder
    {
        public Guid SellOrderID { get; set; }

        [Required()]
        public string? StockSymbol { get; set; }
        [Required()]
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(0, 10000000)]
        public uint? Quantity { get; set; }

        [Range(0, 1000000)]
        public double? Price { get; set; }
    }
}
