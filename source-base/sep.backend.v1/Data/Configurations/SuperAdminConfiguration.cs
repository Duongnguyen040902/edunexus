using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class SuperAdminConfiguration : IEntityTypeConfiguration<SuperAdmin>
    {
        public void Configure(EntityTypeBuilder<SuperAdmin> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.ToTable("SuperAdmin");
            
        }
    }
}
