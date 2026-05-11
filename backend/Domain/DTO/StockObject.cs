namespace backend.Domain.DTO
{
    public class StockObject
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
