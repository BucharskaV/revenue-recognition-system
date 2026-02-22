using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Infrastructure.Data.Configurations;

public class SubscriptionPaymentConfiguration : IEntityTypeConfiguration<SubscriptionPayment>
{
    public void Configure(EntityTypeBuilder<SubscriptionPayment> b)
    {
        b.ToTable("SubscriptionPayments");
        b.HasKey(p => p.Id);
        b.Property(p => p.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        b.Property(p => p.PaymentDate)
            .IsRequired()
            .HasColumnType("datetime");
    }
}