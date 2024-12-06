using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations;

public class BlacklistConfiguration : IEntityTypeConfiguration<Blacklist>
{
    public void Configure(EntityTypeBuilder<Blacklist> builder)
    {
        builder.ToTable("Blacklists");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.AccessToken)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(x => x.RefreshToken)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ExpireDate)
            .HasDefaultValueSql("NOW()")
            .IsRequired();
    }
}