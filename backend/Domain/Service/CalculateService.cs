using backend.Domain.Contract;
using backend.Domain.DTO;
using backend.Domain.Entity;
using backend.Domain.Repository;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace backend.Domain.Service
{
    public class CalculateService(IBomRepository bomRepository, IStockRepository stockRepository, 
        IOrderRepository orderRepository, ITreeService treeService) 
        : ICalculateService
    {
        public async Task<Dictionary<int, int>> CalculateRemainderStock()
        {
            var stocks = await stockRepository.Get();

            Dictionary<int, int> stockRemainders = [];
            foreach (var stock in stocks)
            {
                if (stockRemainders.ContainsKey(stock.ItemId))
                    stockRemainders[stock.ItemId] += (stock.Operation == OperationType.Coming) ? stock.Count : -stock.Count;
                else
                    stockRemainders[stock.ItemId] = stock.Count;
            }
            return stockRemainders;
        }

        public RequiredItem CreateRequiredItem(int itemId, int requiredCount, int stockCount)
        {
            var required = new RequiredItem
            {
                ItemId = itemId,
                RequiredCount = requiredCount,
                StockCount = stockCount
            };
            required.NeedCount = required.RequiredCount - required.StockCount <= 0 ? 0 : required.RequiredCount - required.StockCount;
            return required;
        }

        public async Task<List<RequiredItem>> GetRequiredItemsByRootId(int rootId, int requiredCount)
        {
            var node = await treeService.GetTreeNodeByItemId(rootId);
            var bom = await bomRepository.GetByComponentId(rootId);
            var stockRemainders = await CalculateRemainderStock();         

            if (!stockRemainders.ContainsKey(rootId))
                stockRemainders[rootId] = 0;

            var requiredItem = CreateRequiredItem(rootId, bom == null ? 1 * requiredCount : bom.Count * requiredCount, stockRemainders[rootId]);
            List<RequiredItem> requiredItems = [requiredItem];

            foreach (var child in node.Children)
                requiredItems.AddRange(await GetRequiredItemsByRootId(child.Id, requiredItem.NeedCount));
            return requiredItems;
        }

        public async Task<List<RequiredItem>> CalculateRequiredItems()
        {
            var openOrders = (await orderRepository.GetAsync()).Where(o => o.Status == Status.Open).ToList();
            List<OrderString> orderStrings = [];
            foreach (var order in openOrders)
                orderStrings.AddRange(await orderRepository.GetStringsByOrderIdAsync(order.Id));
                
            var stockRemainders = await CalculateRemainderStock();

            List<RequiredItem> requiredItems = [];
            foreach (var orderString in orderStrings)
                requiredItems.AddRange(await GetRequiredItemsByRootId(orderString.ItemId, orderString.Count));

            List<RequiredItem> requiredItemsByOrderStrings = requiredItems.GroupBy(ri => ri.ItemId).Select(g => new RequiredItem {
                ItemId = g.Key,
                RequiredCount = g.Sum(ri => ri.RequiredCount),
                StockCount = stockRemainders.ContainsKey(g.Key) ? stockRemainders[g.Key] : 0
            }).ToList();
                
            foreach (var requiredItem in requiredItemsByOrderStrings)
                requiredItem.NeedCount = (requiredItem.RequiredCount - requiredItem.StockCount) <= 0 ? 0 : requiredItem.RequiredCount - requiredItem.StockCount;

            return requiredItemsByOrderStrings.OrderBy(ri => ri.ItemId).ToList();
        } 
    }
}
