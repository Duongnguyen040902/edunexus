namespace sep.backend.v1.Data.Entities
{
    public class FeatureAccess : BaseEntity
    {
        public int Id { get; set; }
        public int FeatureId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public virtual Feature? Feature { get; set; }
        public virtual SubscriptionPlan? SubscriptionPlan { get; set; }
    }
}
