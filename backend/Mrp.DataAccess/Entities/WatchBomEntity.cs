namespace Mrp.DataAccess.Entities
{
    public class WatchBomEntity
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public int ChildId { get; set; }

        public int Count { get; set; } = 0;

        public ItemEntity Parent { get; set; } = null!;

        public ItemEntity Child { get; set; } = null!;
    }
}
