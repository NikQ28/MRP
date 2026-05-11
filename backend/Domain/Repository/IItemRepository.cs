using backend.Domain.Entity;

namespace backend.Domain.Repository
{
    public interface IItemRepository
    {
        Task<Item> Create(Item item);
        Task<int> Delete(int id);
        Task<List<Item>> Get();
        Task<Item> GetById(int id);
        Task<List<int>> GetChildrenIdByRootId(int rootId);
        Task<int> Update(int id, string name, string? description);
    }
}