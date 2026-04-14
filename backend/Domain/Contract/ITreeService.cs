using backend.Domain.DTO;

namespace backend.Domain.Contract
{
    public interface ITreeService
    {
        Task<int> DeleteBranchFromNodeId(int nodeId);
        Task<List<TreeNode>> GetChildrenByNodeId(int id);
        Task<List<int>> GetMaterialIdsByRootId(int rootId);
        Task<TreeNode> GetTreeNodeByItemId(int itemId);
        Task<List<int>> GetItemIdsByNodeId(int nodeId);
    }
}