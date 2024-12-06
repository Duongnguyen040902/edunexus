using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class PupilScoreConfiguration : IEntityTypeConfiguration<PupilScore>
    {
        public void Configure(EntityTypeBuilder<PupilScore> builder)
        {
            builder.ToTable("PupilScores");
            builder.HasKey(x => new {x.PupilId, x.SemesterId,x.SubjectId});
            builder.Property(x => x.Score).IsRequired();
            builder.HasOne(x => x.Pupil)
                .WithMany(x => x.PupilScores)
                .HasForeignKey(x => x.PupilId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Semester)
                .WithMany(x => x.StudentScores)
                .HasForeignKey(x => x.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x=> x.Subject)
                .WithMany(x => x.PupilScores)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
