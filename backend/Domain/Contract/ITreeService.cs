using backend.Domain.DTO;

namespace backend.Domain.Contract
{
    public interface ITreeService
    {
        Task<int> DeleteBranchFromNodeId(int nodeId);
        Task<List<TreeNode>> GetChildrenByNodeId(int id);
        Task<TreeNode> GetTreeNodeByItemId(int itemId);
        Task<List<int>> GetItemIdsByNodeId(int nodeId);
    }
}