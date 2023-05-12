using StocksApp.Models;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTO;

namespace StocksApp.Services
{
    public class StockService : IStockService
    {
        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;

        public StockService()
        {
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();
        }

        public BuyOrderResponse CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if(buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }
            if (buyOrderRequest.Price == 0 || buyOrderRequest.Price >= 10000)
            {
                throw new ArgumentException(nameof(buyOrderRequest));
            }
            if (buyOrderRequest.Quantity == 0 || buyOrderRequest.Quantity >= 100000)
            {
                throw new ArgumentException(nameof(buyOrderRequest));
            }
            if (buyOrderRequest.StockSymbol == null)
            {
                throw new ArgumentException(nameof(buyOrderRequest));
            }
            if (buyOrderRequest.DateAndTimeOfOrder!.Value.Year >= 2000)
            {
                throw new ArgumentException(nameof(buyOrderRequest));
            }

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            buyOrder.BuyOrderID = Guid.NewGuid();

            _buyOrders.Add(buyOrder);

            return buyOrder.ToBuyOrderResponse();
        }

        public SellOrderResponse CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(sellOrderRequest));
            }
            if (sellOrderRequest.Price == 0 || sellOrderRequest.Price >= 10000)
            {
                throw new ArgumentException(nameof(sellOrderRequest));
            }
            if (sellOrderRequest.Quantity == 0 || sellOrderRequest.Quantity >= 100000)
            {
                throw new ArgumentException(nameof(sellOrderRequest));
            }
            if (sellOrderRequest.StockSymbol == null)
            {
                throw new ArgumentException(nameof(sellOrderRequest));
            }
            if (sellOrderRequest.DateAndTimeOfOrder!.Value.Year >= 2000)
            {
                throw new ArgumentException(nameof(sellOrderRequest));
            }

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            sellOrder.SellOrderID = Guid.NewGuid();

            _sellOrders.Add(sellOrder);

            return sellOrder.ToSellOrderResponse();
        }

        public List<BuyOrderResponse> GetBuyOrders()
        {
            return _buyOrders.Select(b => b.ToBuyOrderResponse()).ToList();
        }

        public List<SellOrderResponse> GetSellOrders()
        {
            return _sellOrders.Select(s => s.ToSellOrderResponse()).ToList();
        }
    }
}
