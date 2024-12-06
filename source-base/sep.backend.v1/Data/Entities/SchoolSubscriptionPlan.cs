namespace sep.backend.v1.Data.Entities
{
    public class SchoolSubscriptionPlan : BaseEntity
    {
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual School? School { get; set; }
        public virtual SubscriptionPlan? SubscriptionPlan { get; set; }
        public virtual ICollection<Invoice>? Invoices { get; set; }
    }
}
