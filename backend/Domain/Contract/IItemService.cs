using backend.Domain.Entity;

namespace backend.Domain.Contract
{
    public interface IItemService
    {
        Task<Item> CreateItem(Item item);
        Task<int> DeleteItem(int id);
        Task<List<Item>> GetAllItems();
        Task<int> UpdateItem(int id, string name, string? description);
    }
}