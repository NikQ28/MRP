using backend.Domain.DTO;
using backend.Domain.Entity;

namespace backend.Domain.Contract
{
    public interface IOrderService
    {
        Task<int> CreateOrderObjectAsync(OrderRequest request);
        Task<int> DeleteOrderObjectAsync(int orderId);
        Task<List<OrderObject>> GetAllOrderObjectsAsync();
        Task<int> UpdateOrderObjectAsync(int orderId, OrderRequest request);
    }
}