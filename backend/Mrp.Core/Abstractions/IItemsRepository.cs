using Mrp.Core.Models;

namespace Mrp.Core.Abstractions
{
    public interface IItemsRepository
    {
        Task<int> Create(Item item);
        Task<int> Delete(int id);
        Task<List<Item>> Get();
        Task<int> Update(int id, string name, string? description);
    }
}