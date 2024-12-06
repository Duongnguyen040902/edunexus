using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class FeatureAccessesConfiguration : IEntityTypeConfiguration<FeatureAccess>
    {
        public void Configure(EntityTypeBuilder<FeatureAccess> builder)
        {
            builder.ToTable("FeatureAccesses");
            builder.HasKey(x => new {x.FeatureId, x.SubscriptionPlanId, x.Id});

            builder.HasOne(x => x.Feature)
                .WithMany(x => x.FeatureAccesses)
                .HasForeignKey(x => x.FeatureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SubscriptionPlan)
                .WithMany(x => x.FeatureAccesses)
                .HasForeignKey(x => x.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
