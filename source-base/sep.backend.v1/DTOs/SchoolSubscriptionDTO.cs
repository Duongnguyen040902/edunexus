using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs
{
    public class SchoolSubscriptionDTO
    {
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string? SubscriptionPlanName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public List<InvoiceDTO>? Invoices { get; set; }
    }
}
