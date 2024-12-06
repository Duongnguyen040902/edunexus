using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class NotificationImageConfiguration : IEntityTypeConfiguration<NotificationImage>
    {
        public void Configure(EntityTypeBuilder<NotificationImage> builder)
        {
            builder.ToTable("NotificationImages");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Url).IsRequired();
            builder.HasOne(x => x.Notification)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
