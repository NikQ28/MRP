using Mrp.Core.Abstractions;
using Mrp.Core.Models.DTO;
using Mrp.DataAccess.Repositories;

namespace Mrp.Application.Services
{
    public class TreeService(TreeRepository treeRepository) : ITreeService
    {
        private readonly TreeRepository _treeRepository = treeRepository;

        public Task<ItemNode?> GetTree(int rootId) => _treeRepository.GetTree(rootId);
    }
}