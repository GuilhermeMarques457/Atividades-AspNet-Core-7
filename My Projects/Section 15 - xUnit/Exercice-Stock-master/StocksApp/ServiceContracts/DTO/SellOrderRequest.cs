using StocksApp.Models;
using System.ComponentModel.DataAnnotations;

namespace StocksApp.ServiceContracts.DTO
{
    public class SellOrderRequest
    {

        [Required()]
        public string StockSymbol { get; set; }
        [Required()]
        public string StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(0, 100000)]
        public uint? Quantity { get; set; }

        [Range(0, 10000)]
        public double? Price { get; set; }

        public SellOrder ToSellOrder()
        {
            return new SellOrder()
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                Price = Price,
                Quantity = Quantity,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
            };
        }
    }
}
