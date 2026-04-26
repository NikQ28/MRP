namespace backend.Domain.DTO
{
    public class RequiredItem
    {
        public int ItemId { get; set; }
        public int RequiredCount { get; set; } = 0;
        public int StoredCount { get; set; } = 0;
        public int NeedToProduce { get; set; } = 0;
    }
}