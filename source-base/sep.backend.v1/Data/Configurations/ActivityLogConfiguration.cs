using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            builder.ToTable("ActivityLogs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Action).IsRequired().HasMaxLength(50);
            builder.Property(x => x.TableName).IsRequired().HasMaxLength(50);
            builder.HasOne(x => x.User)
                .WithMany(x => x.ActivityLogs)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
