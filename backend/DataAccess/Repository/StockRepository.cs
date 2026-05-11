using backend.Domain.Entity;
using backend.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.DataAccess.Repository
{
    public class StockRepository(MrpContext context) : IStockRepository
    {
        public async Task<List<Stock>> Get() => await context.Stocks.AsNoTracking().ToListAsync();
        public async Task<Stock> GetById(int id) => await context.Stocks.FirstAsync(s => s.Id == id);
        public async Task<Stock> GetByItemId(int itemId) => await context.Stocks.FirstAsync(s => s.ItemId == itemId);
        public async Task<Stock> Create(Stock stock)
        {
            context.Stocks.Add(stock);
            await context.SaveChangesAsync();
            return stock;
        }
        public async Task<int> Update(int id, int itemId, int count, OperationType operation, DateTime dateTime)
        {
            await context.Stocks
                .Where(i => i.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(s => s.ItemId, itemId)
                .SetProperty(s => s.Count, count)
                .SetProperty(s => s.Operation, operation)
                .SetProperty(s => s.Datetime, dateTime));
            return id;
        }
        public async Task<int> Delete(int id)
        {
            await context.Stocks
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
