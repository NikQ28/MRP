namespace backend.Domain.Entity
{
    public class Item
    {
        public int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
