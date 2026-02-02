using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Infrastructure.Data.Configurations;

public class IndividualConfiguration : IEntityTypeConfiguration<Individual>
{
    public void Configure(EntityTypeBuilder<Individual> b)
    {
        b.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        b.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(50);
        b.Property(c => c.PESEL)
            .IsRequired()
            .HasMaxLength(11);
    }
}