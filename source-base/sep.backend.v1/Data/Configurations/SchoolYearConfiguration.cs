using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class SchoolYearConfiguration : IEntityTypeConfiguration<SchoolYear>
    {
        public void Configure(EntityTypeBuilder<SchoolYear> builder)
        {
            builder.ToTable("SchoolYears");
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.HasOne(x => x.School)
                .WithMany(x=> x.SchoolYears)
                .HasForeignKey(x => x.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
