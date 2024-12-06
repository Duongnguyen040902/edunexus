using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class NotificationCategoryConfiguration : IEntityTypeConfiguration<NotificationCategory>
    {
        public void Configure(EntityTypeBuilder<NotificationCategory> builder)
        {
            builder.ToTable("NotificationCategories");
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            
        }
    }
}
