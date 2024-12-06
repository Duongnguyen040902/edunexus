using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
    {
        public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
        {
            builder.ToTable("AttendanceRecords");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Pupil)
                .WithMany(x => x.AttendanceRecords)
                .HasForeignKey(x => x.PupilId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Class)
                .WithMany(x => x.AttendanceRecords)
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Club)
                .WithMany(x => x.AttendanceRecords)
                .HasForeignKey(x => x.ClubId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Bus)
                .WithMany(x => x.AttendanceRecords)
                .HasForeignKey(x => x.BusId)
                .OnDelete(DeleteBehavior.Restrict);
            // Ensure uniqueness for pupil attendance per day and attendance type
            builder.HasIndex(x => new { x.PupilId, x.CreatedDate, x.AttendanceType })
                .IsUnique();
        }
    }
}
