using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class SchoolSubscriptionPlanConfiguration : IEntityTypeConfiguration<SchoolSubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<SchoolSubscriptionPlan> builder)
        {
            builder.ToTable("SchoolSubscriptionPlans");
            
            builder.HasKey(x =>  x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.School)
                .WithMany(x => x.SchoolSubscriptionPlans)
                .HasForeignKey(x => x.SchoolId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(x => x.SubscriptionPlan)
                .WithMany(x => x.SchoolSubscriptionPlans)
                .HasForeignKey(x => x.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}