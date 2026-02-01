using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Client> Clients { get; set; }
    public DbSet<Individual> Individuals { get; set; }
    public DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(b =>
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
        });
        modelBuilder.Entity<Individual>(b =>
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
            b.HasQueryFilter(i => !i.IsDeleted);
        });
        modelBuilder.Entity<Company>(b =>
        {
            b.Property(c => c.CompanyName)
                .IsRequired()
                .HasMaxLength(50);
            b.Property(c => c.KRSNumber)
                .IsRequired()
                .HasMaxLength(10);
        });
    }
}