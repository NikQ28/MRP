using backend.Domain.Contract;
using backend.Domain.DTO;
using backend.Domain.Entity;
using backend.Domain.Repository;

namespace backend.Domain.Service
{
    public class StockService(IStockRepository stockRepository, IItemRepository itemRepository) : IStockService
    {
        public async Task<List<Stock>> GetAllStocks() => await stockRepository.Get();
        public async Task<Stock> CreateStock(Stock stock) => await stockRepository.Create(stock);
        public async Task<int> UpdateStock(int id, int itemId, int count, OperationType operation, DateTime dateTime) =>
            await stockRepository.Update(id, itemId, count, operation, dateTime);
        public async Task<int> DeleteStock(int id) => await stockRepository.Delete(id);

        public async Task<List<StockObject>?> GetStateStocksByDate(DateTime dateTime)
        {
            var stocks = (await stockRepository.Get())
                .Where(s => s.Datetime.Date <= dateTime.Date)
                .GroupBy(s => s.ItemId)
                .ToDictionary(g => g.Key, g => g.Sum(s => s.Operation == OperationType.Coming ? s.Count : -s.Count));

            List<StockObject> stateStocks = [];
            foreach (var stock in stocks)
                stateStocks.Add(new StockObject 
                { 
                    ItemId = stock.Key, 
                    Name = (await itemRepository.GetById(stock.Key)).Name, 
                    Count = stock.Value 
                });
            return stateStocks;
        }
    }
}
