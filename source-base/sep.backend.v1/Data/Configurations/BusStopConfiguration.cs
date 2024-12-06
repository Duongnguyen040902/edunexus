using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class BusStopConfiguration : IEntityTypeConfiguration<BusStop>
    {
        public void Configure(EntityTypeBuilder<BusStop> builder)
        {
            builder.ToTable("BusStops");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.PickUpTime).HasColumnType("time(7)").IsRequired();
            builder.Property(x => x.ReturnTime).HasColumnType("time(7)").IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.HasOne(x=> x.Route)
                .WithMany(x => x.BusStops)
                .HasForeignKey(x => x.BusRouteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
