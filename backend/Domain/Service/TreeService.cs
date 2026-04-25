using backend.Domain.Contract;
using backend.Domain.DTO;
using backend.Domain.Repository;

namespace backend.Domain.Service
{
    public class TreeService(IItemRepository itemRepository, IBomRepository bomRepository, IStockRepository stockRepository, IOrderRepository orderRepository) : ITreeService
    {
        public async Task<TreeNode> GetTreeNodeByItemId(int itemId)
        {
            var nodeItem = await itemRepository.GetById(itemId);
            var bom = await bomRepository.GetByComponentId(itemId);
            var children = await GetChildrenByNodeId(itemId);
            var node = new TreeNode
            {
                Id = itemId,
                Name = nodeItem.Name,
                Count = bom != null ? bom.Count : 1,
                Children = children
            };
            return node;
        }

        public async Task<List<TreeNode>> GetChildrenByNodeId(int nodeId)
        {
            var childIds = await itemRepository.GetChildrenIdByRootId(nodeId);
            List<TreeNode> children = [];
            foreach (var childId in childIds)
                children.Add(await GetTreeNodeByItemId(childId));
            return children;
        }

        public async Task<int> DeleteBranchFromNodeId(int nodeId)
        {
            var stocks = await stockRepository.Get();
            var children = await GetChildrenByNodeId(nodeId);

            var itemIds = await GetItemIdsByNodeId(nodeId);
            var itemWithStock = itemIds.Where(id => stocks.Any(s => s.ItemId == id)).ToList();

            var orderStrings = await orderRepository.GetAllStringsAsync();
            var itemWithOrderStrings = itemIds.Where(id => orderStrings.Any(os => os.ItemId == id)).ToList();

            if (itemWithStock.Count == 0 && itemWithOrderStrings.Count == 0)
            {
                var bom = await bomRepository.GetByComponentId(nodeId);

                foreach (var child in children)
                    await Task.WhenAll(DeleteBranchFromNodeId(child.Id));

                if (bom != null)
                    await bomRepository.Delete(bom.Id);

                await itemRepository.Delete(nodeId);
            }

            return nodeId;
        }

        public async Task<List<int>> GetItemIdsByNodeId(int nodeId)
        {
            var childrenId = await itemRepository.GetChildrenIdByRootId(nodeId);
            List<int> itemIds = [nodeId];

            foreach (var childId in childrenId)
                itemIds.AddRange(await GetItemIdsByNodeId(childId));    
            return itemIds;
        }

        public async Task<List<int>> GetMaterialIdsByRootId(int rootId)
        {
            var childrenId = await itemRepository.GetChildrenIdByRootId(rootId);
            if (childrenId.Count == 0)
                return [rootId];

            List<int> materialIds = [];

            foreach (var childId in childrenId)
                materialIds.Add((await GetMaterialIdsByRootId(childId))[0]);
            return materialIds;
        }
    }
}
