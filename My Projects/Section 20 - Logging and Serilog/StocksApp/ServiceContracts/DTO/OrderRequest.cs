using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class OrderRequest
    {
        
        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(0, 1000000)]
        public uint? Quantity { get; set; }

        public string? OrderType { get; set; }

        [Range(0, 1000000)]
        public double? Price { get; set; }

        public Order ToOrder()
        {
            return new Order()
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                Price = Price,
                Quantity = Quantity,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                OrderType = OrderType
            };
        }
    }
}
