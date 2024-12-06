using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class PupilConfiguration : IEntityTypeConfiguration<Pupil>
    {
        public void Configure(EntityTypeBuilder<Pupil> builder)
        {
            
            builder.ToTable("Pupils");
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
            builder.HasOne(x=> x.School)
                .WithMany(x => x.Pupils)
                .HasForeignKey(x => x.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
