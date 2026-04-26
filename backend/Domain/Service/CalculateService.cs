using backend.Domain.Contract;
using backend.Domain.DTO;
using backend.Domain.Entity;
using backend.Domain.Repository;

namespace backend.Domain.Service
{
    public class CalculateService(IBomRepository bomRepository, IStockRepository stockRepository, IOrderRepository orderRepository) : ICalculateService
    {
        public async Task<List<RequiredItem>> Calculate()
        {
            var boms = await bomRepository.Get();
            var orders = (await orderRepository.GetAsync()).Where(o => o.Status == Status.Open).ToList();
            List<OrderString> orderStrings = [];
            foreach (var order in orders)
                orderStrings.AddRange(await orderRepository.GetStringsByOrderIdAsync(order.Id));
            
            var stock = (await stockRepository.Get()).GroupBy(s => s.ItemId).ToDictionary(g => g.Key, g => g.Sum(s => s.Operation == OperationType.Coming ? s.Count : -s.Count));
            var children = boms.GroupBy(b => b.ParentId).ToDictionary(g => g.Key, g => g.ToList());
            List<RequiredItem> required = [];
           
            foreach (var orderString in orderStrings)
                CalculateItem(orderString.ItemId, orderString.Count, required, stock, children);

            return required.OrderBy(r => r.ItemId).ToList();
        }

        private void CalculateItem(int itemId, int requiredCount, List<RequiredItem> required, Dictionary<int, int> stock, Dictionary<int, List<Bom>> children)
        {
            var requiredIds = required.Select(r => r.ItemId).Distinct().ToList();
            if (!requiredIds.Contains(itemId))
                required.Add(new RequiredItem { ItemId = itemId });

            var requiredItem = required.Where(r => r.ItemId == itemId).ToList()[0];
            requiredItem.RequiredCount += requiredCount;

            stock[itemId] = stock.ContainsKey(itemId) ? stock[itemId] : 0;
            var takeFromStocks = Math.Min(requiredCount, stock[itemId]);

            requiredItem.StockCount += takeFromStocks;
            stock[itemId] -= takeFromStocks;

            var needToProduce = requiredCount - takeFromStocks;
            requiredItem.NeedCount += needToProduce;
            if (children.TryGetValue(itemId, out var boms))
                foreach (var bom in boms)
                    CalculateItem(bom.ComponentId, needToProduce * bom.Count, required, stock, children);
        }
    }
}