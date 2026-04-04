using backend.Domain.Entity;

namespace backend.Domain.Contract
{
    public interface IBomService
    {
        Task<Bom> CreateBom(Bom bom);
        Task<int> DeleteBom(int id);
        Task<List<Bom>> GetAllBoms();
        Task<int> UpdateBom(int id, int parentId, int componentId, int count);
    }
}