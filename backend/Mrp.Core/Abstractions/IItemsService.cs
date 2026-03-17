using Mrp.Core.Models;

namespace Mrp.Core.Abstractions
{
    public interface IItemsService
    {
        Task<int> CreateItem(Item item);
        Task<int> DeleteItem(int id);
        Task<List<Item>> GetAllItems();
        Task<int> UpdateItem(int id, string name, string? description);
    }
}