namespace Mrp.API.Contracts
{
    public record ItemsRequest
        (
            string Name,
            string? Description
        );
}
