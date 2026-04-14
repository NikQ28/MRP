namespace backend.Domain.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Creation { get; set; }
        public DateTime Execution { get; set; }
        public Status Status { get; set; }
    }
    public enum Status
    {
        Open, 
        Close
    }
}
