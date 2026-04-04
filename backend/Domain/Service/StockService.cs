using backend.Domain.Contract;
using backend.Domain.Entity;
using backend.Domain.Repository;

namespace backend.Domain.Service
{
    public class StockService(IStockRepository stockRepository) : IStockService
    {
        public async Task<List<Stock>> GetAllStocks() => await stockRepository.Get();
        public async Task<Stock> CreateStock(Stock stock) => await stockRepository.Create(stock);
        public async Task<int> UpdateStock(int id, int itemId, int count, OperationType operation, DateTime dateTime) =>
            await stockRepository.Update(id, itemId, count, operation, dateTime);
        public async Task<int> DeleteStock(int id) => await stockRepository.Delete(id);
    }
}
