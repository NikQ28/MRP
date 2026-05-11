using backend.Domain.Entity;
using backend.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace backend.DataAccess.Repository
{
    public class OrderRepository(MrpContext context) : IOrderRepository
    {
        public async Task<List<Order>> GetAsync() => await context.Orders.AsNoTracking().ToListAsync();
        public async Task<Order> GetByIdAsync(int id) => await context.Orders.FirstAsync(o => o.Id == id);
        public async Task<int> CreateAsync(Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return order.Id;
        }
        public async Task<int> UpdateAsync(int id, DateTime creation, DateTime execution, Status status)
        {
            await context.Orders
                .Where(o => o.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(o => o.Creation, creation)
                .SetProperty(o => o.Execution, execution)
                .SetProperty(o => o.Status, status));
            return id;
        }
        public async Task<int> DeleteAsync(int id)
        {
            await context.Orders
                .Where(o => o.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }

        public async Task<List<OrderString>> GetStringsByOrderIdAsync(int orderId) => await context.OrderStrings.Where(os => os.OrderId == orderId).ToListAsync();
        public async Task<List<OrderString>> GetAllStringsAsync() => await context.OrderStrings.AsNoTracking().ToListAsync();
        public async Task<int> CreateStringAsync(OrderString orderString)
        {
            context.OrderStrings.Add(orderString);
            await context.SaveChangesAsync();
            return orderString.Id;
        }
        public async Task<int> UpdateStringAsync(int id, int itemId, int count)
        {
            await context.OrderStrings
                .Where(os => os.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(os => os.Count, count));
            return id;
        }
        public async Task<int> DeleteStringAsync(int id)
        {
            await context.OrderStrings
                .Where(os => os.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
