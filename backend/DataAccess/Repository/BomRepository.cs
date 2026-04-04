using backend.Domain.Entity;
using backend.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace backend.DataAccess.Repository
{
    public class BomRepository(MrpContext context) : IBomRepository
    {
        public async Task<List<Bom>> Get() => await context.Boms.AsNoTracking().ToListAsync();
        public async Task<Bom> GetById(int id) => await context.Boms.FirstAsync(b => b.Id == id);
        public async Task<List<Bom>> GetByParentId(int parentId) => await context.Boms.Where(b => b.ParentId == parentId).ToListAsync(); 
        public async Task<Bom?> GetByComponentId(int componentId) =>
            await context.Boms.FirstOrDefaultAsync(b => b.ComponentId == componentId);
        public async Task<Bom> Create(Bom bom)
        {
            context.Boms.Add(bom);
            await context.SaveChangesAsync();
            return bom;
        }
        public async Task<int> Update(int id, int parentId, int componentId, int count)
        {
            await context.Boms
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.ParentId, parentId)
                .SetProperty(b => b.ComponentId, componentId)
                .SetProperty(b => b.Count, count));
            return id;
        }
        public async Task<int> Delete(int id)
        {
            await context.Boms
                .Where(b => b.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
