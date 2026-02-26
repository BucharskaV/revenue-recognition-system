using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>

{
    public void Configure(EntityTypeBuilder<Employee> b)
    {
        b.ToTable("Employees");
        b.HasKey(e => e.Id);
        b.Property(e => e.Login)
            .IsRequired();
        b.Property(e => e.Password)
            .IsRequired();
        b.Property(e => e.Role)
            .IsRequired();
        b.Property(e => e.Salt)
            .IsRequired();
        b.Property(e => e.RefreshToken)
            .IsRequired();
        b.Property(e => e.RefreshTokenExp)
            .IsRequired()
            .HasColumnType("datetime");
    }
}