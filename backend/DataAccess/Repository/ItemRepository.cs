using backend.Domain.Entity;
using backend.Domain.Repository;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace backend.DataAccess.Repository
{
    public class ItemRepository(MrpContext context) : IItemRepository
    {
        public async Task<List<Item>> Get() => await context.Items.AsNoTracking().ToListAsync();
        public async Task<Item> GetById(int id) => await context.Items.FirstAsync(i => i.Id == id);
        public async Task<List<int>> GetChildrenByRootId(int rootId) =>
              await context.Boms.Where(c => c.ParentId == rootId).Select(c => c.ComponentId).ToListAsync();
        public async Task<Item> Create(Item item)
        {
            context.Items.Add(item);
            await context.SaveChangesAsync();
            return item;
        }
        public async Task<int> Update(int id, string name, string? description)
        {
            await context.Items
                .Where(i => i.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(i => i.Name, name)
                .SetProperty(i => i.Description, description));
            return id;
        }
        public async Task<int> Delete(int id)
        {
            await context.Items
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
