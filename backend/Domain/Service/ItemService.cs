using backend.Domain.Contract;
using backend.Domain.Entity;
using backend.Domain.Repository;

namespace backend.Domain.Service
{
    public class ItemService(IItemRepository itemRepository) : IItemService
    {
        public async Task<List<Item>> GetAllItems() => await itemRepository.Get();
        public async Task<Item> CreateItem(Item item) => await itemRepository.Create(item);
        public async Task<int> UpdateItem(int id, string name, string? description) => await itemRepository.Update(id, name, description);
        public async Task<int> DeleteItem(int id) => await itemRepository.Delete(id);
    }
}
