namespace backend.Domain.Entity
{
    public class Bom
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ComponentId { get; set; }
        public int Count { get; set; }
    }
}
