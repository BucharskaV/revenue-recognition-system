using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Infrastructure.Data.Configurations;

public class SoftwareSystemConfiguration : IEntityTypeConfiguration<SoftwareSystem>
{
    public void Configure(EntityTypeBuilder<SoftwareSystem> b)
    {
        b.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(50);
        b.Property(s => s.Description)
            .IsRequired()
            .HasMaxLength(255);
        b.Property(s => s.CurrentVersion)
            .IsRequired()
            .HasMaxLength(50);
        b.Property(s => s.Category)
            .IsRequired();
        b.Property(s => s.UpfrontPrice)
            .HasPrecision(18, 2);
        b.Property(s => s.MonthlySubscriptionPrice)
            .HasPrecision(18, 2);
    }
}