using backend.Domain.Entity;

namespace backend.Domain.DTO
{
    public class OrderObject
    {
        public int OrderId { get; set; }
        public DateTime Creation { get; set; }
        public DateTime Execution { get; set; }
        public Status Status { get; set; }
        public List<OrderString> OrderStrings { get; set; } = [];
    }
}
