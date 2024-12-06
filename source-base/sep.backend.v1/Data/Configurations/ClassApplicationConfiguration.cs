using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;


namespace sep.backend.v1.Data.Configurations
{
    public class ClassApplicationConfiguration : IEntityTypeConfiguration<ClassApplication>
    {
        public void Configure(EntityTypeBuilder<ClassApplication> builder)
        {
            builder.ToTable("ClassApplication");
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Pupil)
                .WithMany(x => x.ClassApplications)
                .HasForeignKey(x => x.PupilId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Semester)
                .WithMany(x => x.ClassApplications)
                .HasForeignKey(x => x.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.ClassApplicationCategory)
                .WithMany(x => x.ClassApplications)
                .HasForeignKey(x => x.ApplicationCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
