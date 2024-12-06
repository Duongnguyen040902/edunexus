using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class TimeTableConfiguration : IEntityTypeConfiguration<TimeTable>
    {
        public void Configure(EntityTypeBuilder<TimeTable> builder)
        {
            builder.ToTable("TimeTables");
            builder.HasKey(x => new {x.ClassId , x.SubjectId,x.TimeSlotId,x.DayOfWeek,x.SemesterId });
            builder.HasOne(x=>x.Semester)
                .WithMany(x => x.TimeTables)
                .HasForeignKey(x => x.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Class)
                .WithMany(x => x.TimeTables)
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.TimeSlot)
                .WithMany(x => x.TimeTables)
                .HasForeignKey(x => x.TimeSlotId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Subject)
                .WithMany(x => x.TimeTables)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
