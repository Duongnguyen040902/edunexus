using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
           builder.ToTable("Subjects");
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Code).IsRequired().HasMaxLength(10);
            builder.HasOne(x => x.School)
                .WithMany(x => x.Subjects)
                .HasForeignKey(x => x.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
