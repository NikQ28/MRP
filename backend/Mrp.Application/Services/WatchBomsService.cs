using Mrp.Core.Abstractions;
using Mrp.Core.Models;

namespace Mrp.Application.Services
{
    public class WatchBomsService(IWatchBomsRepository watchBomsRepository) : IWatchBomsService
    {
        private readonly IWatchBomsRepository _watchBomsRepository = watchBomsRepository;

        public async Task<List<WatchBom>> GetAllWatchBoms()
        {
            return await _watchBomsRepository.Get();
        }
        public async Task<int> CreateWatchBom(WatchBom watchBom)
        {
            return await _watchBomsRepository.Create(watchBom);
        }
        public async Task<int> UpdateWatchBom(int id, int parentId, int childId, int count)
        {
            return await _watchBomsRepository.Update(id, parentId, childId, count);
        }
        public async Task<int> DeleteWatchBom(int id)
        {
            return await _watchBomsRepository.Delete(id);
        }
    }
}
