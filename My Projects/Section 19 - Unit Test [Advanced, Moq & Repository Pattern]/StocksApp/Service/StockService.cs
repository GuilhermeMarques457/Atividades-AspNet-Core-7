using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;


        public StockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<OrderResponse> CreateOrder(OrderRequest? OrderRequest)
        {
            if (OrderRequest == null)
            {
                throw new ArgumentNullException(nameof(OrderRequest));
            }
            if (OrderRequest.Price == 0 || OrderRequest.Price >= 1000000)
            {
                throw new ArgumentException(nameof(OrderRequest));
            }
            if (OrderRequest.Quantity == 0 || OrderRequest.Quantity >= 100000)
            {
                throw new ArgumentException(nameof(OrderRequest));
            }
            if (OrderRequest.StockSymbol == null)
            {
                throw new ArgumentException(nameof(OrderRequest));
            }
            if (OrderRequest.DateAndTimeOfOrder!.Value.Year <= 2000)
            {
                throw new ArgumentException(nameof(OrderRequest));
            }

            Order Order = OrderRequest.ToOrder();

            Order.OrderID = Guid.NewGuid();

            await _stockRepository.CreateOrder(Order);

            return Order.ToOrderResponse();
        }

        public async Task<List<OrderResponse>> GetOrders()
        {
            List<Order> orders = await _stockRepository.GetOrders();
            return orders.ToList().Select(o => o.ToOrderResponse()).ToList();
        }

    }
}
