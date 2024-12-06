using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public int Status { get; set; }
    }
    public class PaymentDetailDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
}
