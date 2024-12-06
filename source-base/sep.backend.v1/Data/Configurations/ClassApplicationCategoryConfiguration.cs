using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class ClassApplicationCategoryConfiguration : IEntityTypeConfiguration<ClassApplicationCategory>
    {
        public void Configure(EntityTypeBuilder<ClassApplicationCategory> builder)
        {
            builder.ToTable("ClassApplicationCategory");
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
        }
    }
}
