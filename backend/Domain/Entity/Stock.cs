namespace backend.Domain.Entity
{
    public class Stock
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; }
        public OperationType Operation { get; set; }
        public DateTime Datetime { get; set; }
    }
    public enum OperationType
    {
        Приход, 
        Расход
    };
}
