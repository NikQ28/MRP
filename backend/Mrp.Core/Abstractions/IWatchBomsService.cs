using Mrp.Core.Models;

namespace Mrp.Core.Abstractions
{
    public interface IWatchBomsService
    {
        Task<int> CreateWatchBom(WatchBom watchBom);
        Task<int> DeleteWatchBom(int id);
        Task<List<WatchBom>> GetAllWatchBoms();
        Task<int> UpdateWatchBom(int id, int parentId, int childId, int count);
    }
}