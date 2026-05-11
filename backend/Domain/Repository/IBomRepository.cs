using backend.Domain.Entity;

namespace backend.Domain.Repository
{
    public interface IBomRepository
    {
        Task<Bom> Create(Bom bom);
        Task<int> Delete(int id);
        Task<List<Bom>> Get();
        Task<Bom> GetById(int id);
        Task<List<Bom>> GetByParentId(int parentId);
        Task<Bom?> GetByComponentId(int componentId);
        Task<int> Update(int id, int parentId, int componentId, int count);
    }
}