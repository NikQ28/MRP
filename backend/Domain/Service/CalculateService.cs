using backend.Controller;
using backend.Domain.Contract;
using backend.Domain.DTO;
using backend.Domain.Entity;
using backend.Domain.Repository;

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

        public RequiredItem CreateRequiredItem(int itemId, int count)
        {
            var required = new RequiredItem
            {
                ItemId = itemId,
                RequiredCount = count
            };
            return required;
        }

        public async Task<List<RequiredItem>> GetRequiredItemsByRootId(int rootId, int requiredCount)
        {
            var node = await treeService.GetTreeNodeByItemId(rootId);
            var bom = await bomRepository.GetByComponentId(rootId);

            var requiredItem = CreateRequiredItem(rootId, bom == null ? 1 * requiredCount : bom.Count * requiredCount);
            List<RequiredItem> requiredItems = [requiredItem];

            foreach (var child in node.Children)
                requiredItems.AddRange(await GetRequiredItemsByRootId(child.Id, requiredItem.RequiredCount));
            return requiredItems;
        }

        public async Task<List<RequiredItem>> CalculateRemaindedItems()
        {
            var stockRemainders = await CalculateRemainderStock();
            List<RequiredItem> storedItems = [];
            foreach (var item in stockRemainders)
                storedItems.AddRange(await GetRequiredItemsByRootId(item.Key, item.Value));

            List<RequiredItem> storedItemsByStocks = storedItems.GroupBy(ri => ri.ItemId).Select(g => new RequiredItem
            {
                ItemId = g.Key,
                RequiredCount = g.Sum(ri => ri.RequiredCount)
            }).ToList();

            return storedItemsByStocks;
        }

        public async Task<List<RequiredItem>> CalculateRequiredItems()
        {
            var openOrders = (await orderRepository.GetAsync()).Where(o => o.Status == Status.Open).ToList();
            List<OrderString> orderStrings = [];
            foreach (var order in openOrders)
                orderStrings.AddRange(await orderRepository.GetStringsByOrderIdAsync(order.Id));

            List<RequiredItem> requiredItems = [];
            foreach (var orderString in orderStrings)
                requiredItems.AddRange(await GetRequiredItemsByRootId(orderString.ItemId, orderString.Count));

            List<RequiredItem> requiredItemsByOrderStrings = requiredItems.GroupBy(ri => ri.ItemId).Select(g => new RequiredItem 
            {
                ItemId = g.Key,
                RequiredCount = g.Sum(ri => ri.RequiredCount),
            }).ToList();

            return requiredItemsByOrderStrings;
        } 

        public async Task<List<RequiredItem>> Calculate()
        {
            var requiredItems = await CalculateRequiredItems();
            var storedItems = await CalculateRemaindedItems();

            foreach (var requiredItem in requiredItems)
                foreach (var storedItem in storedItems)
                    if (requiredItem.ItemId == storedItem.ItemId)
                    {
                        requiredItem.StoredCount = storedItem.RequiredCount;
                        requiredItem.NeedToProduce = requiredItem.RequiredCount - storedItem.RequiredCount;
                    }

            return requiredItems.OrderBy(ri => ri.ItemId).ToList();
        }
    }
}
