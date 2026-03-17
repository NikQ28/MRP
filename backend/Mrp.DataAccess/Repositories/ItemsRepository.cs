using Microsoft.EntityFrameworkCore;
using Mrp.Core.Models;
using Mrp.Core.Abstractions;
using Mrp.DataAccess.Entities;

namespace Mrp.DataAccess.Repositories
{
    public class ItemsRepository(MrpDbContext context) : IItemsRepository
    {
        private readonly MrpDbContext _context = context;

        public async Task<List<Item>> Get()
        {
            var itemEntities = await _context.Items
                .AsNoTracking()
                .ToListAsync();
            var items = itemEntities
                .Select(i => Item.Create(i.Id, i.Name, i.Description).item)
                .ToList();
            return items;
        }
        public async Task<int> Create(Item item)
        {
            var itemEntity = new ItemEntity
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description
            };
            _context.Items.Add(itemEntity);
            await _context.SaveChangesAsync();
            return itemEntity.Id;
        }
        public async Task<int> Update(int id, string name, string? description)
        {
            await _context.Items
                .Where(i => i.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(i => i.Name, name)
                .SetProperty(i => i.Description, description));
            await _context.SaveChangesAsync();
            return id;
        }
        public async Task<int> Delete(int id)
        {
            await _context.Items
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return id;
        }
    }
}
