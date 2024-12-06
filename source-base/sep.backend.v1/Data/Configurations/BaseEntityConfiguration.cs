using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class BaseEntityConfiguration : IEntityTypeConfiguration<BaseEntity>
    {
        public void Configure(EntityTypeBuilder<BaseEntity> builder)
        {
            builder.Property(x => x.CreatedDate)
                   .HasColumnType("timestamp")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .IsRequired();

            builder.Property(x => x.UpdatedDate)
                   .HasColumnType("timestamp")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .IsRequired(false);

            builder.Property(x => x.CreatedBy)
                   .HasColumnType("bigint")
                   .IsRequired(false);

            builder.Property(x => x.UpdatedBy)
                   .HasColumnType("bigint")
                   .IsRequired(false);
        }
    }
}