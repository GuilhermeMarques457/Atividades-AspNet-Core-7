using System.ComponentModel.DataAnnotations;

namespace StocksApp.Models
{
    public class Order
    {
        [Key]
        public Guid OrderID { get; set; }
        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(0, 1000000)]
        public uint? Quantity { get; set; }

        public string? OrderType { get; set; } 

        [Range(0, 1000000)]
        public double? Price { get; set; }
    }
}
