using backend.Domain.Entity;

namespace backend.Domain.DTO
{
    public record StockRequest
    (
        int ItemId,
        int Count,
        OperationType Operation,
        DateTime Datetime
    );
}
