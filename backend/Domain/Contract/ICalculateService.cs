using backend.Domain.DTO;

namespace backend.Domain.Contract
{
    public interface ICalculateService
    {
        Task<List<RequiredItem>> Calculate();
    }
}
