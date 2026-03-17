namespace Mrp.API.Contracts
{
    public record WatchBomsResponse
        (
            int Id,
            int ParentId,
            int ChildId,
            int Count
        );
}
