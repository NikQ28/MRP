using backend.Domain.Entity;

namespace backend.Domain.DTO
{
    public record OrderRequest
    (
        DateTime Creation,
        DateTime Execution,
        Status Status,
        List<OrderStringRequest> OrderStrings
    );
}
