using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Infrastructure.Data.Configurations;

public class SoftwareSystemConfiguration : IEntityTypeConfiguration<SoftwareSystem>
{
    public void Configure(EntityTypeBuilder<SoftwareSystem> b)
    {
        b.ToTable("SoftwareSystems");
        b.HasKey(c => c.Id);
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
        b.Property(s => s.OneYearLicensePrice)
            .HasColumnType("decimal(18,2)");
        b.HasMany(c => c.Discounts)
            .WithOne(e => e.SoftwareSystem)
            .HasForeignKey(e => e.SoftwareSystemId)
            .OnDelete(DeleteBehavior.Cascade);
        b.HasMany(c => c.Contracts)
            .WithOne(e => e.SoftwareSystem)
            .HasForeignKey(e => e.SoftwareSystemId)
            .OnDelete(DeleteBehavior.Cascade);
        b.HasMany(c => c.Subscriptions)
            .WithOne(e => e.SoftwareSystem)
            .HasForeignKey(e => e.SoftwareSystemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}