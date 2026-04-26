namespace backend.Domain.DTO
{
    public class RequiredItem
    {
        public int ItemId { get; set; }
        public int RequiredCount { get; set; } = 0;
        public int StockCount { get; set; } = 0;
        public int NeedCount { get; set; } = 0;
    }
}