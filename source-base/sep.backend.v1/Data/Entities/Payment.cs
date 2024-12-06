namespace sep.backend.v1.Data.Entities
{
    public class Payment : BaseEntity
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public int Status { get; set; }
        public virtual Invoice? Invoice { get; set; }
    }
}
