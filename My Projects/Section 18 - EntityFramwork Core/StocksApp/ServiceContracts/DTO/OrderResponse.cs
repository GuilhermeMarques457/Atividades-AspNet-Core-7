using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class OrderResponse
    {
        public Guid OrderID { get; set; }

        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(0, 1000000)]
        public uint? Quantity { get; set; }

        public string? OrderType { get; set; }

        [Range(0, 1000000)]
        public double? Price { get; set; }

        public double? TradeAmount { get; set; }
    }

    public static class OrderExtensions
    {
        public static OrderResponse ToOrderResponse(this Order Order)
        {
            return new OrderResponse()
            {
                StockSymbol = Order.StockSymbol,
                StockName = Order.StockName,
                Price = Order.Price,
                Quantity = Order.Quantity,
                DateAndTimeOfOrder = Order.DateAndTimeOfOrder,
                OrderID = Order.OrderID,
                TradeAmount = Order.Price * Order.Quantity,
                OrderType = Order.OrderType,

            };
        }
    }
}
