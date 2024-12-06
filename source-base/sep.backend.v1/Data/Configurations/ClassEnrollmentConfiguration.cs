using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class ClassEnrollmentConfiguration : IEntityTypeConfiguration<ClassEnrollment>
    {
        public void Configure(EntityTypeBuilder<ClassEnrollment> builder)
        {
            builder.ToTable("ClassEnrollments");
            builder.HasKey(x => new { x.Id });
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.ClassEnrollments)
                .HasForeignKey(x => x.TeacherId);
            builder.HasOne(x => x.Pupil)
                .WithMany(x => x.PupilClasses)
                .HasForeignKey(x => x.PupilId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Class)
                .WithMany(x => x.ClassEnrollments)
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Semester)
                .WithMany(x => x.ClassEnrollments)
                .HasForeignKey(x => x.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => new { x.PupilId, x.SemesterId })
                .IsUnique();
            builder.HasIndex(x => new { x.TeacherId, x.SemesterId })
                .IsUnique();
                
        }
    }
}
