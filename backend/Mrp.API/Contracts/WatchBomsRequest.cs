namespace Mrp.API.Contracts
{
    public record WatchBomsRequest
        (
            int ParentId,
            int ChildId,
            int Count
        );
}
