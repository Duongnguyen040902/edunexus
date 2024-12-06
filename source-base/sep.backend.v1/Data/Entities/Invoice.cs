namespace sep.backend.v1.Data.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public int SchoolSubscriptionPlanId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public int Status { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
        public virtual SchoolSubscriptionPlan? SchoolSubscriptionPlans { get; set; }
    }
}
