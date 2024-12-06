using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class SemesterConfiguration : IEntityTypeConfiguration<Semester>
    {

        public void Configure(EntityTypeBuilder<Semester> builder)
        {
            builder.ToTable("Semesters");
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.SemesterName).IsRequired().HasMaxLength(100);
            builder.HasOne(x => x.SchoolYear)
                .WithMany(x => x.Semesters)
                .HasForeignKey(x => x.SchoolYearId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
