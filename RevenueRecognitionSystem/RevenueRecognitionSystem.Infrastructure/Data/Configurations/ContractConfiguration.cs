using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Infrastructure.Data.Configurations;

public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> b)
    {
        b.ToTable("Contracts");
        b.HasKey(c => c.Id);
        b.Property(c => c.SoftwareSystemId)
            .IsRequired();
        b.Property(c => c.ClientId)
            .IsRequired();
        b.Property(c => c.SoftwareVersion)
            .IsRequired()
            .HasMaxLength(50);
        b.Property(c => c.StartDate)
            .IsRequired()
            .HasColumnType("datetime");
        b.Property(c => c.EndDate)
            .IsRequired()
            .HasColumnType("datetime");
        b.Property(c => c.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime");
        b.Property(c => c.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        b.Property(c => c.UpdateYears)
            .IsRequired()
            .HasColumnType("int");
        b.HasMany(c => c.Payments)
            .WithOne(e => e.Contract)
            .HasForeignKey(e => e.ContractId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}