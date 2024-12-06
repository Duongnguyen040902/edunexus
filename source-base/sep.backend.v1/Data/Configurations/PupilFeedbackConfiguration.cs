using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class PupilFeedbackConfiguration : IEntityTypeConfiguration<PupilFeedback>
    {
        public void Configure(EntityTypeBuilder<PupilFeedback> builder)
        {
            builder.ToTable("PupilFeedbacks");
            builder.HasKey(x => new {x.PupilId,x.SemesterId});
            builder.Property(x => x.PupilId).IsRequired();
            builder.Property(x=>x.Description).IsRequired().HasMaxLength(2000);
            builder.HasOne(x => x.Pupil)
                .WithMany(x => x.PupilFeedbacks)
                .HasForeignKey(x => x.PupilId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Semester)
                .WithMany(x => x.StudentFeedbacks)
                .HasForeignKey(x => x.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}
