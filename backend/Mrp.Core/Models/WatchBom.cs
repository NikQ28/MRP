using System.Data.Entity.Validation;

namespace Mrp.Core.Models
{
    public class WatchBom
    {
        private WatchBom(int id, int parentId, int childId, int count)
        {
            Id = id;
            ParentId = parentId;
            ChildId = childId;
            Count = count;
        }

        public int Id { get; }

        public int ParentId { get; }
        
        public int ChildId { get; }
        
        public int Count { get; } = 0;

        public static (WatchBom watchBom, string error) Create(int id, int parentId, int childId, int count)
        {
            var error = string.Empty;
            if (count < 1)
                error = "Count can't be less then 1!";
            var watchBom = new WatchBom(id, parentId, childId, count);
            return (watchBom, error);
        }
    }
}
