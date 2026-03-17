using Microsoft.EntityFrameworkCore;
using Mrp.Core.Abstractions;
using Mrp.Core.Models;
using Mrp.Core.Models.DTO;
using Mrp.DataAccess.Entities;

namespace Mrp.DataAccess.Repositories
{
    public class TreeRepository(MrpDbContext context)
    {
        private readonly MrpDbContext _context = context;

        public async Task<ItemNode?> GetTree(int rootId)
        {
            var items = await _context.Items
                .AsNoTracking()
                .ToListAsync();

            var links = await _context.WatchBoms
                .AsNoTracking()
                .ToListAsync();

            var itemsById = items.ToDictionary(i => i.Id);

            if (!itemsById.TryGetValue(rootId, out var rootItem))
                return null;

            var childrenByParent = links
                .GroupBy(w => w.ParentId)
                .ToDictionary(g => g.Key, g => g.ToList());

            return BuildNode(rootItem.Id, 1, itemsById, childrenByParent);
        }

        private ItemNode BuildNode(
            int itemId,
            int count,
            IReadOnlyDictionary<int, ItemEntity> itemsById,
            IReadOnlyDictionary<int, List<WatchBomEntity>> childrenByParent)
        {
            if (!itemsById.TryGetValue(itemId, out var item))
                throw new InvalidOperationException($"Item with id {itemId} not found.");

            if (!childrenByParent.TryGetValue(itemId, out var childLinks))
                childLinks = [];

            var children = childLinks
                .Select(link => BuildNode(link.ChildId, link.Count, itemsById, childrenByParent))
                .ToList();

            return new ItemNode
            {
                Id = item.Id,
                Name = item.Name,
                Count = count,
                Children = children
            };
        }
    }
}