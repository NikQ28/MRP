namespace Mrp.DataAccess.Entities
{
    public class ItemEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public List<WatchBomEntity> Children { get; set; } = [];

        public List<WatchBomEntity> Parents { get; set; } = [];
    }
}
