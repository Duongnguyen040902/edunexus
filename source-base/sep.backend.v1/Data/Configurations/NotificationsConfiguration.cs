using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class NotificationsConfiguration : IEntityTypeConfiguration<Notifications>
    {
        public void Configure(EntityTypeBuilder<Notifications> builder)
        {
            builder.ToTable("Notifications");
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Descriptions).IsRequired().HasMaxLength(500);
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x=> x.Class)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x=>x.SchoolYear)
                .WithMany(x=>x.Notifications)
                .HasForeignKey(x => x.SchoolYearId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
