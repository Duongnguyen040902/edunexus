using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class ClubEnrollmentConfiguration : IEntityTypeConfiguration<ClubEnrollment>
    {
        public void Configure(EntityTypeBuilder<ClubEnrollment> builder)
        {
            builder.ToTable("ClubEnrollments");
            builder.HasKey(x => new {x.Id});
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Club)
                .WithMany(x => x.ClubEnrollments)
                .HasForeignKey(x => x.ClubId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Pupil)
                .WithMany(x => x.ClubEnrollments)
                .HasForeignKey(x => x.PupilId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.ClubEnrollments)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Semester)
                .WithMany(x => x.ClubEnrollments)
                .HasForeignKey(x => x.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
