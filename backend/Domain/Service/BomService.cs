using backend.Domain.Contract;
using backend.Domain.Entity;
using backend.Domain.Repository;

namespace backend.Domain.Service
{
    public class BomService(IBomRepository bomRepository) : IBomService
    {
        public async Task<List<Bom>> GetAllBoms() => await bomRepository.Get();
        public async Task<Bom> CreateBom(Bom bom) => await bomRepository.Create(bom);
        public async Task<int> UpdateBom(int id, int parentId, int componentId, int count) => await bomRepository.Update(id, parentId, componentId, count);
        public async Task<int> DeleteBom(int id) => await bomRepository.Delete(id);
    }
}
