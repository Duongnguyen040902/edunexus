namespace sep.backend.v1.DTOs
{
    public class SubscriptionPlanDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public int? MaxActiveAccounts { get; set; }
        public List<FeatureDTO>? Features { get; set; }
    }

    public class SubscriptionPlanDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public int? MaxActiveAccounts { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
