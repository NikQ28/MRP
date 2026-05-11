using backend.Domain.Entity;

namespace backend.Domain.Repository
{
    public interface IOrderRepository
    {
        Task<int> CreateAsync(Order order);
        Task<int> CreateStringAsync(OrderString orderString);
        Task<int> DeleteAsync(int id);
        Task<int> DeleteStringAsync(int id);
        Task<List<Order>> GetAsync();
        Task<List<OrderString>> GetAllStringsAsync();
        Task<Order> GetByIdAsync(int id);
        Task<List<OrderString>> GetStringsByOrderIdAsync(int orderId);
        Task<int> UpdateAsync(int id, DateTime creation, DateTime execution, Status status);
        Task<int> UpdateStringAsync(int id, int itemId, int count);
    }
}