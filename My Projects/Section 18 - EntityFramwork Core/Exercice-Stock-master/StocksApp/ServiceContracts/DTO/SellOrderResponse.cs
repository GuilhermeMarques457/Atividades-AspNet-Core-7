using StocksApp.Models;

namespace StocksApp.ServiceContracts.DTO
{
    public class SellOrderResponse
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

            if (obj.GetType() != typeof(SellOrderResponse))
            {
                return false;
            }

            SellOrderResponse sellOrderResponse = (SellOrderResponse)obj;

            return this.BuyOrderID == sellOrderResponse.BuyOrderID
                && this.Quantity == sellOrderResponse.Quantity
                && this.Price == sellOrderResponse.Price
                && this.StockSymbol == sellOrderResponse?.StockSymbol
                && this.StockName == sellOrderResponse?.StockName
                && this.DateAndTimeOfOrder == sellOrderResponse?.DateAndTimeOfOrder;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    public static class SellOrderExtensions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                Price = sellOrder.Price,
                Quantity = sellOrder.Quantity,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                BuyOrderID = sellOrder.SellOrderID,
                TradeAmout = sellOrder.Price * sellOrder.Quantity,

            };
        }
    }
}
