using StocksApp.Models;
using System.ComponentModel.DataAnnotations;

namespace StocksApp.ServiceContracts.DTO
{
    public class BuyOrderRequest
    {


        public string? StockSymbol { get; set; }
       
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(1, 100000)]
        public uint? Quantity { get; set; }

        [Range(1, 10000)]
        public double? Price { get; set; }

        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder()
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
