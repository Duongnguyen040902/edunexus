namespace sep.backend.v1.Data.Entities
{
    public class SubscriptionPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public int MaxActiveAccounts { get; set; }
        public virtual ICollection<SchoolSubscriptionPlan>? SchoolSubscriptionPlans { get; set; }
        public virtual ICollection<FeatureAccess>? FeatureAccesses { get; set; }
    }
}
