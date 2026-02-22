using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Infrastructure.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> b)
    {
        b.ToTable("Clients");
        b.HasKey(c => c.Id);
        b.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(255);
        b.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(50);
        b.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(9);
        b.HasDiscriminator<string>("ClientType")
            .HasValue<Company>("Company")
            .HasValue<Individual>("Individual")
            .IsComplete();
        b.HasMany(c => c.Contracts)
            .WithOne(e => e.Client)
            .HasForeignKey(e => e.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
        b.HasMany(c => c.Subscriptions)
            .WithOne(e => e.Client)
            .HasForeignKey(e => e.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}