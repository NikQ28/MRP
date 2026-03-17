namespace Mrp.API.Contracts
{
    public record ItemsResponse
        (
            int Id,
            string Name,
            string? Description
        );
}
