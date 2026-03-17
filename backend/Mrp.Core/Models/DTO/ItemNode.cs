namespace Mrp.Core.Models.DTO
{
    public class ItemNode
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
        public List<ItemNode> Children { get; set; } = [];
    }
}
