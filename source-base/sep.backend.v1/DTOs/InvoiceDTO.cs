using sep.backend.v1.Common.Enums;

namespace sep.backend.v1.DTOs
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int Status { get; set; }
        public int schoolId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string StatusName { get; set; }
        public string? SchoolName { get; set; }
        public string? SubscriptionPlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double? TotalAmount { get; set; }
        public ICollection<PaymentDTO>? Payments { get; set; }
    }

    public class CreateInvoiceDTO
    {
        public int SubscriptionPlanId { get; set; }
        public int SchoolId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public string? PaymentMethod { get; set; }
        public int Status { get; set; } = (int)InvoiceStatuses.Pending;
    }

}

    

