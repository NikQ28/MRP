using Microsoft.EntityFrameworkCore;
using Mrp.Core.Abstractions;
using Mrp.Core.Models;
using Mrp.DataAccess.Entities;

namespace Mrp.DataAccess.Repositories
{
    public class WatchBomsRepository(MrpDbContext context) : IWatchBomsRepository
    {
        private readonly MrpDbContext _context = context;

        public async Task<List<WatchBom>> Get()
        {
            var watchBomEntities = await _context.WatchBoms
                .AsNoTracking()
                .ToListAsync();
            var watchBoms = watchBomEntities
                .Select(w => WatchBom.Create(w.Id, w.ParentId, w.ChildId, w.Count).watchBom)
                .ToList();
            return watchBoms;
        }
        public async Task<int> Create(WatchBom watchBom)
        {
            var watchBomEntity = new WatchBomEntity
            {
                Id = watchBom.Id,
                ParentId = watchBom.ParentId,
                ChildId = watchBom.ChildId,
                Count = watchBom.Count
            };
            _context.WatchBoms.Add(watchBomEntity);
            await _context.SaveChangesAsync();
            return watchBomEntity.Id;
        }
        public async Task<int> Update(int id, int parentId, int childId, int count)
        {
            await _context.WatchBoms
                .Where(w => w.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(w => w.ParentId, parentId)
                .SetProperty(w => w.ChildId, childId)
                .SetProperty(w => w.Count, count));
            await _context.SaveChangesAsync();
            return id;
        }
        public async Task<int> Delete(int id)
        {
            await _context.WatchBoms
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return id;
        }
    }
}
