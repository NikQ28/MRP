using backend.Domain.Entity;

namespace backend.Domain.DTO
{
    public record OrderResponse
    (
        DateTime Creation,
        DateTime Execution,
        Status Status,
        List<OrderString> OrderStrings
    );
}
