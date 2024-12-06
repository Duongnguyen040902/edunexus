using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class TeacherSubjectConfiguration : IEntityTypeConfiguration<TeacherSubject>
    {
        public void Configure(EntityTypeBuilder<TeacherSubject> builder)
        {
            builder.ToTable("TeacherSubjects");
            builder.HasKey(x => new {  x.TeacherId, x.SubjectId } );
            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.TeacherSubjects)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Subject)
                .WithMany(x => x.TeacherSubjects)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
