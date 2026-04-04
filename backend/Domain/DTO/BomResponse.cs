namespace backend.Domain.DTO
{
    public record BomResponse
    (
        int Id,
        int ParentId,
        int ComponentId,
        int Count
    );
}
