using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Infrastructure.Data.Configurations;

public class CompanyConfiguration: IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> b)
    {
        b.Property(c => c.CompanyName)
            .IsRequired()
            .HasMaxLength(50);
        b.Property(c => c.KRSNumber)
            .IsRequired()
            .HasMaxLength(10);
    }
}