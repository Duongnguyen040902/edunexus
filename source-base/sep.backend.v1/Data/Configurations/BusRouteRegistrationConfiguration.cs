using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class BusRouteRegistrationConfiguration : IEntityTypeConfiguration<BusRouteRegistration>
    {
        public void Configure(EntityTypeBuilder<BusRouteRegistration> builder)
        {
            builder.ToTable("BusRouteRegistrations");
            builder.HasKey(x => new {x.PupilId,x.BusStopId,x.SemesterId } );
            builder.HasOne(x => x.BusStop)
                .WithMany(x => x.Registrations)
                .HasForeignKey(x => x.BusStopId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Pupil)
                .WithMany(x => x.BusRouteRegistrations)
                .HasForeignKey(x => x.PupilId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Semester)
                .WithMany(x => x.BusRouteRegistrations)
                .HasForeignKey(x => x.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
