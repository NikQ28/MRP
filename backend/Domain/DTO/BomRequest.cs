namespace backend.Domain.DTO
{
    public record BomRequest
    (
        int ParentId,
        int ComponentId,
        int Count
    );

}
