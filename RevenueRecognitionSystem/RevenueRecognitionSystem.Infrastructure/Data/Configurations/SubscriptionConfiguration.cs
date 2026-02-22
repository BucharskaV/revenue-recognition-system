using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Infrastructure.Data.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> b)
    {
        b.ToTable("Subscriptions");
        b.HasKey(c => c.Id);
        b.Property(c => c.SoftwareSystemId)
            .IsRequired();
        b.Property(c => c.ClientId)
            .IsRequired();
        b.Property(c => c.StartDate)
            .IsRequired()
            .HasColumnType("datetime");
        b.Property(c => c.RenewalPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        b.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(20);
        b.Property(c => c.RenewalPeriodInMonths)
            .IsRequired()
            .HasColumnType("int");
        b.HasMany(c => c.SubscriptionPayments)
            .WithOne(e => e.Subscription)
            .HasForeignKey(e => e.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}