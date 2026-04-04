using backend.Domain.Entity;

namespace backend.Domain.Contract
{
    public interface IStockService
    {
        Task<Stock> CreateStock(Stock stock);
        Task<int> DeleteStock(int id);
        Task<List<Stock>> GetAllStocks();
        Task<int> UpdateStock(int id, int itemId, int count, OperationType operation, DateTime dateTime);
    }
}