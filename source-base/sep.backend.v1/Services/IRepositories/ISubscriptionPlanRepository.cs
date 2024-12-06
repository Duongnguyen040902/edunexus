using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface ISubscriptionPlanRepository : IRepository<SubscriptionPlan>
    {
        Task<List<SubscriptionPlan>> GetAllSubscriptionAsync();
    }
}
