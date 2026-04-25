namespace backend.Domain.DTO
{
    public class RequiredItem
    {
        public int ItemId { get; set; }
        public int RequiredCount { get; set; }
        public int StockCount { get; set; }
        public int NeedCount { get; set; } = 0;
    }
}