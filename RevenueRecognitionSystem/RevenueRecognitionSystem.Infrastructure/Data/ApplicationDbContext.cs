using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Enums;
using RevenueRecognitionSystem.Infrastructure.Data.Configurations;

namespace RevenueRecognitionSystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Client> Clients { get; set; }
    public DbSet<Individual> Individuals { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<SoftwareSystem?> SoftwareSystems { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Contract> Contracts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new IndividualConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new SoftwareSystemConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountConfiguration());
        modelBuilder.ApplyConfiguration(new ContractConfiguration());
            
        insertInitialData(modelBuilder);
    }

    private void insertInitialData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>().HasData(
            new
            {
                Id = 3,
                Address = "Gdansk, ul. Portowa 12",
                Email = "info@techsoft.com",
                PhoneNumber = "111222333",
                CompanyName = "TechSoft Ltd.",
                KRSNumber = "0000123456",
                ClientType = "Company"
            },
            new
            {
                Id = 4,
                Address = "Poznan, ul. Edukacyjna 7",
                Email = "contact@educorp.com",
                PhoneNumber = "444555666",
                CompanyName = "EduCorp Sp. z o.o.",
                KRSNumber = "0000654321",
                ClientType = "Company"
            }
        );
        modelBuilder.Entity<Individual>().HasData(
            new
            {
                Id = 10,
                Address = "Warsaw, ul. Centralna 10",
                Email = "jan.kowalski@example.com",
                PhoneNumber = "123456789",
                FirstName = "Jan",
                LastName = "Kowalski",
                PESEL = "90010112345",
                IsDeleted = false,
                ClientType = "Individual"
            },
            new
            {
                Id = 20,
                Address = "Krakow, ul. Zielona 5",
                Email = "anna.nowak@example.com",
                PhoneNumber = "987654321",
                FirstName = "Anna",
                LastName = "Nowak",
                PESEL = "85050567890",
                IsDeleted = false,
                ClientType = "Individual"
            }
        );
        modelBuilder.Entity<SoftwareSystem>().HasData(
            new SoftwareSystem
            {
                Id = 1,
                Name = "FinTrack",
                Description = "Financial software",
                CurrentVersion = "1.2.3",
                Category = SoftwareCategory.Finance,
                OneYearLicensePrice = 5000
            },
            new SoftwareSystem
            {
                Id = 2,
                Name = "LearnMate",
                Description = "Education platform",
                CurrentVersion = "3.0.0",
                Category = SoftwareCategory.Education,
                OneYearLicensePrice = 11000
            }
        );
        modelBuilder.Entity<Discount>().HasData(
            new Discount
            {
                Id = 1,
                Name = "Black Friday Discount",
                Percentage = 10,
                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 2, 1),
                Target = DiscountTarget.Subscription,
                SoftwareSystemId = 1
            }
        );

    }
}