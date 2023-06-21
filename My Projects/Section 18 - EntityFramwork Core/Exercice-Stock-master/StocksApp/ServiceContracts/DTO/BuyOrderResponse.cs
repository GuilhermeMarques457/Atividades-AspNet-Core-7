using StocksApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace StocksApp.ServiceContracts.DTO
{
    public class BuyOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public uint? Quantity { get; set; }
        public double? Price { get; set; }
        public double? TradeAmout { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(BuyOrderResponse))
            {
                return false;
            }

            BuyOrderResponse buyOrderResponse = (BuyOrderResponse)obj;

            return this.BuyOrderID == buyOrderResponse.BuyOrderID
                && this.Quantity == buyOrderResponse.Quantity
                && this.Price == buyOrderResponse.Price
                && this.StockSymbol == buyOrderResponse?.StockSymbol
                && this.StockName == buyOrderResponse?.StockName
                && this.DateAndTimeOfOrder == buyOrderResponse?.DateAndTimeOfOrder;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    public static class BuyOrderExtensions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder) 
        {
            return new BuyOrderResponse()
            {
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                Price = buyOrder.Price,
                Quantity = buyOrder.Quantity,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                BuyOrderID = buyOrder.BuyOrderID,
                TradeAmout = buyOrder.Price * buyOrder.Quantity,
           
            };
        }
    }
}
