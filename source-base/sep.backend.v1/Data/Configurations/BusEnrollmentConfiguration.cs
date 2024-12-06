using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class BusEnrollmentConfiguration : IEntityTypeConfiguration<BusEnrollment>
    {
        public void Configure(EntityTypeBuilder<BusEnrollment> builder)
        {
            builder.ToTable("BusEnrollments");
            builder.HasKey(x => new {x.Id} );
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Bus)
                .WithMany(x => x.BusEnrollments)
                .HasForeignKey(x => x.BusId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.BusSupervisor)
                .WithMany(x => x.BusEnrollments)
                .HasForeignKey(x => x.BusSupervisorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Pupil)
                .WithMany(x => x.BusEnrollments)
                .HasForeignKey(x => x.PupilId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x=>x.BusStop)
                .WithMany(x => x.BusEnrollments)
                .HasForeignKey(x => x.BusStopId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ensure uniqueness for Pupil within a Semester
            builder.HasIndex(x => new { x.PupilId, x.SemesterId })
                .IsUnique(); // No filter needed in PostgreSQL for nullable fields

            // Ensure uniqueness for Bus Supervisor within a Semester
            builder.HasIndex(x => new { x.BusSupervisorId, x.SemesterId })
                .IsUnique(); // No filter needed in PostgreSQL for nullable fields

        }
    }
}
