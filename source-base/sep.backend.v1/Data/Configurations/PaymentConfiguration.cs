using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.InvoiceId).IsRequired();
            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.PaymentDate).IsRequired();
            builder.Property(x => x.PaymentMethod).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Status).HasMaxLength(50).IsRequired();
            builder.HasOne(x => x.Invoice)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}