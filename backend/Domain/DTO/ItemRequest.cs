using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace backend.Domain.DTO
{
    public record ItemRequest
    (
        string Name,
        string? Description
    );
}
