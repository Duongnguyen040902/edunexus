using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface ISubscriptionPlanService
    {
        Task<List<SubscriptionPlanDTO>> GetAllSubscriptionsAsync();
        Task<SubscriptionPlanDetailDTO> GetSubscriptionsDetailAsync(int subscriptionId);
        Task<List<SubscriptionPlanDTO>> GetAllSubscriptionsByAdminAsync();
        Task<SubscriptionPlanDTO> GetSubscriptionByIdAsync(int id);
        Task<SubscriptionPlanDTO> CreateSubscriptionAsync(SubscriptionPlanDTO subscriptionPlanDTO);
        Task<SubscriptionPlanDTO> UpdateSubscriptionAsync(int id, SubscriptionPlanDTO subscriptionPlanDTO);
    }
}
