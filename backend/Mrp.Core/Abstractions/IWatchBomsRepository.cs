using Mrp.Core.Models;

namespace Mrp.Core.Abstractions
{
    public interface IWatchBomsRepository
    {
        Task<int> Create(WatchBom watchBom);
        Task<int> Delete(int id);
        Task<List<WatchBom>> Get();
        Task<int> Update(int id, int parentId, int childId, int count);
    }
}