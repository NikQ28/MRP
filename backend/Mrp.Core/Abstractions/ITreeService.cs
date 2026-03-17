using Mrp.Core.Models.DTO;

namespace Mrp.Core.Abstractions
{
    public interface ITreeService
    {
        Task<ItemNode?> GetTree(int rootId);
    }
}