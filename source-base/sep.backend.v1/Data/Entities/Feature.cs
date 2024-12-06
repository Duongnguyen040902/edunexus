namespace sep.backend.v1.Data.Entities
{
    public class Feature : BaseEntity
    {
        public int Id { get; set; }
        public string FeatureName { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<FeatureAccess>? FeatureAccesses { get; set; }
    }
}
