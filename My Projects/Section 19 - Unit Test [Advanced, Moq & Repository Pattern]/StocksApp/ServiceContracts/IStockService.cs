using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IStockService
    {
        Task<OrderResponse> CreateOrder(OrderRequest? OrderRequest);

        Task<List<OrderResponse>> GetOrders();

    }
}
