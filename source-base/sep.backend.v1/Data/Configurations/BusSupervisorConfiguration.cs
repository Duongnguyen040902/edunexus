using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class BusSupervisorConfiguration : IEntityTypeConfiguration<BusSupervisor>
    {
        public void Configure(EntityTypeBuilder<BusSupervisor> builder)
        {
            builder.ToTable("BusSupervisors");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasOne(d => d.School)
                .WithMany(p => p.BusSupervisors)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);              
        }       
    }     
}
