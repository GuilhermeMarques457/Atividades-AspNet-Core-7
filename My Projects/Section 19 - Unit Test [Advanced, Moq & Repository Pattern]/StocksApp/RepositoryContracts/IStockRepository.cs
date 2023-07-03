using Entities;

namespace RepositoryContracts
{
    public interface IStockRepository
    {
        Task<Order> CreateOrder(Order Order);
        Task<List<Order>> GetOrders();
    }
}