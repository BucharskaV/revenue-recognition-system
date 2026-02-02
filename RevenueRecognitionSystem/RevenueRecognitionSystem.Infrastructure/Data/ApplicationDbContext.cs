using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Infrastructure.Data.Configurations;

namespace RevenueRecognitionSystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Client> Clients { get; set; }
    public DbSet<Individual> Individuals { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<SoftwareSystem> SoftwareSystems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new IndividualConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new SoftwareSystemConfiguration());
            
        modelBuilder.Entity<Individual>().HasData(
            new Individual
            {
                Id = 10,
                FirstName = "Jan",
                LastName = "Kowalski",
                Address = "Warsaw, ul. Centralna 10",
                Email = "jan.kowalski@example.com",
                PhoneNumber = "123456789",
                PESEL = "90010112345",
                IsDeleted = false
            },
            new Individual
            {
                Id = 20,
                FirstName = "Anna",
                LastName = "Nowak",
                Address = "Krakow, ul. Zielona 5",
                Email = "anna.nowak@example.com",
                PhoneNumber = "987654321",
                PESEL = "85050567890",
                IsDeleted = false
            }
        );

        modelBuilder.Entity<Company>().HasData(
            new Company
            {
                Id = 3,
                CompanyName = "TechSoft Ltd.",
                Address = "Gdansk, ul. Portowa 12",
                Email = "info@techsoft.com",
                PhoneNumber = "111222333",
                KRSNumber = "0000123456"
            },
            new Company
            {
                Id = 4,
                CompanyName = "EduCorp Sp. z o.o.",
                Address = "Poznan, ul. Edukacyjna 7",
                Email = "contact@educorp.com",
                PhoneNumber = "444555666",
                KRSNumber = "0000654321"
            }
        );

        modelBuilder.Entity<SoftwareSystem>().HasData(
            new SoftwareSystem
            {
                Id = 1,
                Name = "FinTrack",
                Description = "Financial software",
                CurrentVersion = "1.2.3",
                Category = Domain.Enums.SoftwareCategory.Finance,
                UpfrontPrice = 499.99m,
                MonthlySubscriptionPrice = 29.99m,
                IdClient = 10
            },
            new SoftwareSystem
            {
                Id = 2,
                Name = "LearnMate",
                Description = "Education platform",
                CurrentVersion = "3.0.0",
                Category = Domain.Enums.SoftwareCategory.Education,
                UpfrontPrice = null,
                MonthlySubscriptionPrice = 19.99m,
                IdClient = 4
            }
        );
    }
}