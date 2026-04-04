using backend.Domain.Entity;

namespace backend.Domain.Repository
{
    public interface IStockRepository
    {
        Task<Stock> Create(Stock stock);
        Task<int> Delete(int id);
        Task<List<Stock>> Get();
        Task<Stock> GetById(int id);
        Task<Stock> GetByItemId(int itemId);
        Task<int> Update(int id, int itemId, int count, OperationType operation, DateTime dateTime);

    }
}