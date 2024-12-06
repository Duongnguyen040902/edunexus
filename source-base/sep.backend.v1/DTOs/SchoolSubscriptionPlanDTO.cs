namespace sep.backend.v1.DTOs
{
    public class SchoolSubscriptionPlanDTO
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionPlanDTO SubscriptionPlan { get; set; }
        public ICollection<InvoiceDTO> Invoices { get; set; }
    }
}
