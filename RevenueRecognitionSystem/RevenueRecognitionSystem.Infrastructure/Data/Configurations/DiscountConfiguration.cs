using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Infrastructure.Data.Configurations;

public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> b)
    {
        b.ToTable("Discounts");
        b.HasKey(c => c.Id);
        b.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);
        b.Property(s => s.Percentage)
            .IsRequired()
            .HasPrecision(18, 2);
        b.Property(c => c.StartDate)
            .IsRequired()
            .HasColumnType("datetime");
        b.Property(c => c.EndDate)
            .IsRequired()
            .HasColumnType("datetime");
        b.Property(s => s.Target)
            .IsRequired();
    }
}