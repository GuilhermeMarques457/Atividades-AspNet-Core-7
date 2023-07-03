using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly StockDbContext _context;

        public StockRepository(StockDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrder(Order Order)
        {
            await _context.Order.AddAsync(Order);
            await _context.SaveChangesAsync();
            return Order;
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _context.Order.Select(o => o).ToListAsync();
        }
    }
}