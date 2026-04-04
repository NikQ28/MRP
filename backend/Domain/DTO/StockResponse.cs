using backend.Domain.Entity;

namespace backend.Domain.DTO
{
    public record StockResponse
    (
        int Id,
        int itemId,
        int Count,
        OperationType Operation,
        DateTime Datetime
    );
}
