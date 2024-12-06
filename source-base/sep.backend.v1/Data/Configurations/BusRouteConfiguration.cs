using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class BusRouteConfiguration : IEntityTypeConfiguration<BusRoute>
    {
        public void Configure(EntityTypeBuilder<BusRoute> builder)
        {
           builder.ToTable("BusRoutes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.HasOne(x=>x.School)
                .WithMany(x => x.BusRoutes)
                .HasForeignKey(x => x.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
