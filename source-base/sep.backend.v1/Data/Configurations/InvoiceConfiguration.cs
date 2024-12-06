using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.DueDate)
                .HasColumnType("timestamp")
                .IsRequired();

            builder.Property(x => x.IssueDate)
                .HasColumnType("timestamp")
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();
            builder.HasOne(x => x.SchoolSubscriptionPlans)
                .WithMany(x => x.Invoices)
                .HasForeignKey(x=>x.SchoolSubscriptionPlanId);
        }
    }
}