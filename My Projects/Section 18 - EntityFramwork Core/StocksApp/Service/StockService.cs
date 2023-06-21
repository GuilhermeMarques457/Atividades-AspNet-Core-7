using Entities;
using Microsoft.EntityFrameworkCore;
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
        private readonly StockDbContext _context;


        public StockService(StockDbContext stockDbContext)
        {
            _context = stockDbContext;
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

            await _context.AddAsync(Order);
            await _context.SaveChangesAsync();

            return Order.ToOrderResponse();
        }

        public async Task<List<OrderResponse>> GetOrders()
        {
            return await _context.Order.Select(o => o.ToOrderResponse()).ToListAsync();
        }

    }
}
