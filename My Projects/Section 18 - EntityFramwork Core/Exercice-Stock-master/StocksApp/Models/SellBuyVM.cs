using StocksApp.ServiceContracts.DTO;

namespace StocksApp.Models
{
    public class SellBuyVM
    {
        public List<SellOrderResponse> SellOrders = new List<SellOrderResponse>();
        public List<BuyOrderResponse> BuyOrders= new List<BuyOrderResponse>();
    }
}
