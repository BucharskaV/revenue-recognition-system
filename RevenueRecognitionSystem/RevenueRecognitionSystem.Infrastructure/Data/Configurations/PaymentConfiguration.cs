using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> b)
    {
        b.ToTable("Payments");
        b.HasKey(p => p.Id);
        b.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(18, 2);
        b.Property(p => p.PaymentDate)
            .IsRequired()
            .HasColumnType("datetime");
    }
}