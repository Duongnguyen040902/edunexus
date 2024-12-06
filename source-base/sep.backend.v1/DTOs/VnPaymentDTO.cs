namespace sep.backend.v1.DTOs
{

    public class VnPaymentResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string TransactionStatus { get; set; }
        public string OrderId { get; set; }
        public string InvoiceId { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
    }

    public class VnPaymentRequestDTO
    {
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public string SubscriptionPlan { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
