using Mrp.Core.Abstractions;
using Mrp.Core.Models;

namespace Mrp.Application.Services
{
    public class ItemsService(IItemsRepository itemsRepository) : IItemsService
    {
        readonly IItemsRepository _itemsRepository = itemsRepository;

        public async Task<List<Item>> GetAllItems()
        {
            return await _itemsRepository.Get();
        }
        public async Task<int> CreateItem(Item item)
        {
            return await _itemsRepository.Create(item);
        }
        public async Task<int> UpdateItem(int id, string name, string? description)
        {
            return await _itemsRepository.Update(id, name, description);
        }
        public async Task<int> DeleteItem(int id)
        {
            return await _itemsRepository.Delete(id);
        }
    }
}
