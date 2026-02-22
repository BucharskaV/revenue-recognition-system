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
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new IndividualConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new SoftwareSystemConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountConfiguration());
        modelBuilder.ApplyConfiguration(new ContractConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriptionPaymentConfiguration());
            
        insertInitialData(modelBuilder);
    }

    private void insertInitialData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>().HasData(
            new { Id = 1, Address = "Gdansk 1", Email = "c1@comp.pl", PhoneNumber = "111111111", CompanyName = "TechSoft", KRSNumber = "0000000001", ClientType = "Company" },
            new { Id = 2, Address = "Gdansk 2", Email = "c2@comp.pl", PhoneNumber = "111111112", CompanyName = "EduCorp", KRSNumber = "0000000002", ClientType = "Company" },
            new { Id = 3, Address = "Gdynia 3", Email = "c3@comp.pl", PhoneNumber = "111111113", CompanyName = "FinGroup", KRSNumber = "0000000003", ClientType = "Company" },
            new { Id = 4, Address = "Warsaw 4", Email = "c4@comp.pl", PhoneNumber = "111111114", CompanyName = "MedTech", KRSNumber = "0000000004", ClientType = "Company" },
            new { Id = 5, Address = "Poznan 5", Email = "c5@comp.pl", PhoneNumber = "111111115", CompanyName = "BuildIT", KRSNumber = "0000000005", ClientType = "Company" },
            new { Id = 6, Address = "Krakow 6", Email = "c6@comp.pl", PhoneNumber = "111111116", CompanyName = "LogiSoft", KRSNumber = "0000000006", ClientType = "Company" },
            new { Id = 7, Address = "Lodz 7", Email = "c7@comp.pl", PhoneNumber = "111111117", CompanyName = "HealthPlus", KRSNumber = "0000000007", ClientType = "Company" },
            new { Id = 8, Address = "Wroclaw 8", Email = "c8@comp.pl", PhoneNumber = "111111118", CompanyName = "CloudNet", KRSNumber = "0000000008", ClientType = "Company" },
            new { Id = 9, Address = "Sopot 9", Email = "c9@comp.pl", PhoneNumber = "111111119", CompanyName = "RetailPro", KRSNumber = "0000000009", ClientType = "Company" },
            new { Id = 10, Address = "Katowice 10", Email = "c10@comp.pl", PhoneNumber = "111111120", CompanyName = "AutoSys", KRSNumber = "0000000010", ClientType = "Company" }
        );

        modelBuilder.Entity<Individual>().HasData(
            new { Id = 11, Address = "Warsaw A", Email = "i1@mail.pl", PhoneNumber = "222222221", FirstName = "Jan", LastName = "Kowalski", PESEL = "90010100001", IsDeleted = false, ClientType = "Individual" },
            new { Id = 12, Address = "Warsaw B", Email = "i2@mail.pl", PhoneNumber = "222222222", FirstName = "Anna", LastName = "Nowak", PESEL = "90010100002", IsDeleted = false, ClientType = "Individual" },
            new { Id = 13, Address = "Krakow C", Email = "i3@mail.pl", PhoneNumber = "222222223", FirstName = "Piotr", LastName = "Zielinski", PESEL = "90010100003", IsDeleted = false, ClientType = "Individual" },
            new { Id = 14, Address = "Lodz D", Email = "i4@mail.pl", PhoneNumber = "222222224", FirstName = "Maria", LastName = "Wojcik", PESEL = "90010100004", IsDeleted = false, ClientType = "Individual" },
            new { Id = 15, Address = "Gdansk E", Email = "i5@mail.pl", PhoneNumber = "222222225", FirstName = "Tomasz", LastName = "Lewandowski", PESEL = "90010100005", IsDeleted = false, ClientType = "Individual" },
            new { Id = 16, Address = "Poznan F", Email = "i6@mail.pl", PhoneNumber = "222222226", FirstName = "Kasia", LastName = "Kaminska", PESEL = "90010100006", IsDeleted = false, ClientType = "Individual" },
            new { Id = 17, Address = "Wroclaw G", Email = "i7@mail.pl", PhoneNumber = "222222227", FirstName = "Marek", LastName = "Kaczmarek", PESEL = "90010100007", IsDeleted = false, ClientType = "Individual" },
            new { Id = 18, Address = "Szczecin H", Email = "i8@mail.pl", PhoneNumber = "222222228", FirstName = "Ewa", LastName = "Mazur", PESEL = "90010100008", IsDeleted = false, ClientType = "Individual" },
            new { Id = 19, Address = "Lublin I", Email = "i9@mail.pl", PhoneNumber = "222222229", FirstName = "Adam", LastName = "Krol", PESEL = "90010100009", IsDeleted = false, ClientType = "Individual" },
            new { Id = 20, Address = "Rzeszow J", Email = "i10@mail.pl", PhoneNumber = "222222230", FirstName = "Olga", LastName = "Dabrowska", PESEL = "90010100010", IsDeleted = false, ClientType = "Individual" }
        );
        
        modelBuilder.Entity<SoftwareSystem>().HasData(
            new SoftwareSystem { Id = 1, Name = "FinTrack", Description = "Finance System", CurrentVersion = "1.0", Category = SoftwareCategory.Finance, OneYearLicensePrice = 5000 },
            new SoftwareSystem { Id = 2, Name = "EduPro", Description = "Education Platform", CurrentVersion = "2.1", Category = SoftwareCategory.Education, OneYearLicensePrice = 7000 },
            new SoftwareSystem { Id = 3, Name = "HealthCareX", Description = "Medical System", CurrentVersion = "3.2", Category = SoftwareCategory.Health, OneYearLicensePrice = 9000 },
            new SoftwareSystem { Id = 4, Name = "BuildMaster", Description = "Construction Management", CurrentVersion = "1.4", Category = SoftwareCategory.Other, OneYearLicensePrice = 8000 },
            new SoftwareSystem { Id = 5, Name = "RetailSuite", Description = "Retail Software", CurrentVersion = "5.0", Category = SoftwareCategory.Other, OneYearLicensePrice = 6500 },
            new SoftwareSystem { Id = 6, Name = "CloudBase", Description = "Cloud Platform", CurrentVersion = "4.0", Category = SoftwareCategory.Other, OneYearLicensePrice = 12000 },
            new SoftwareSystem { Id = 7, Name = "LogiTrack", Description = "Logistics System", CurrentVersion = "2.5", Category = SoftwareCategory.Other, OneYearLicensePrice = 7500 },
            new SoftwareSystem { Id = 8, Name = "AutoManager", Description = "Automotive Software", CurrentVersion = "3.1", Category = SoftwareCategory.Other, OneYearLicensePrice = 6000 },
            new SoftwareSystem { Id = 9, Name = "HRPro", Description = "HR Management", CurrentVersion = "6.2", Category = SoftwareCategory.Other, OneYearLicensePrice = 5500 },
            new SoftwareSystem { Id = 10, Name = "SecureIT", Description = "Security System", CurrentVersion = "1.8", Category = SoftwareCategory.Other, OneYearLicensePrice = 10000 }
        );

        modelBuilder.Entity<Discount>().HasData(
            new Discount { Id = 1, Name = "New Year", Percentage = 10, StartDate = new DateTime(2026,1,1), EndDate = new DateTime(2026,2,1), Target = DiscountTarget.Subscription, SoftwareSystemId = 1 },
            new Discount { Id = 2, Name = "Spring Sale", Percentage = 15, StartDate = new DateTime(2026,2,1), EndDate = new DateTime(2026,4,1), Target = DiscountTarget.Subscription, SoftwareSystemId = 2 },
            new Discount { Id = 3, Name = "Health Promo", Percentage = 5, StartDate = new DateTime(2026,5,1), EndDate = new DateTime(2026,6,1), Target = DiscountTarget.Subscription, SoftwareSystemId = 3 },
            new Discount { Id = 4, Name = "Summer Deal", Percentage = 20, StartDate = new DateTime(2026,7,1), EndDate = new DateTime(2026,8,1), Target = DiscountTarget.Subscription, SoftwareSystemId = 4 },
            new Discount { Id = 5, Name = "Autumn Deal", Percentage = 12, StartDate = new DateTime(2026,9,1), EndDate = new DateTime(2026,10,1), Target = DiscountTarget.Subscription, SoftwareSystemId = 5 },
            new Discount { Id = 6, Name = "Cloud Discount", Percentage = 18, StartDate = new DateTime(2026,1,15), EndDate = new DateTime(2026,3,15), Target = DiscountTarget.Subscription, SoftwareSystemId = 6 },
            new Discount { Id = 7, Name = "Logistics Promo", Percentage = 8, StartDate = new DateTime(2026,2,1), EndDate = new DateTime(2026,3,1), Target = DiscountTarget.Subscription, SoftwareSystemId = 7 },
            new Discount { Id = 8, Name = "Auto Deal", Percentage = 7, StartDate = new DateTime(2026,4,1), EndDate = new DateTime(2026,5,1), Target = DiscountTarget.Subscription, SoftwareSystemId = 8 },
            new Discount { Id = 9, Name = "HR Promo", Percentage = 6, StartDate = new DateTime(2026,6,1), EndDate = new DateTime(2026,7,1), Target = DiscountTarget.Subscription, SoftwareSystemId = 9 },
            new Discount { Id = 10, Name = "Security Week", Percentage = 25, StartDate = new DateTime(2026,11,1), EndDate = new DateTime(2026,12,1), Target = DiscountTarget.Subscription, SoftwareSystemId = 10 }
        );

        modelBuilder.Entity<Contract>().HasData(
            new Contract { Id = 1, ClientId = 1, SoftwareSystemId = 1, SoftwareVersion = "1.0", CreatedAt = DateTime.Now, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10), Price = 4500, UpdateYears = 1, IsPaid = true, IsCancelled = false },
            new Contract { Id = 2, ClientId = 2, SoftwareSystemId = 2, SoftwareVersion = "2.1", CreatedAt = DateTime.Now, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(20), Price = 6000, UpdateYears = 2, IsPaid = true, IsCancelled = false },
            new Contract { Id = 3, ClientId = 3, SoftwareSystemId = 3, SoftwareVersion = "3.2", CreatedAt = DateTime.Now, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(10), Price = 8500, UpdateYears = 1, IsPaid = true, IsCancelled = false }
        );

        modelBuilder.Entity<Subscription>().HasData(
            new Subscription {Id = 1, ClientId = 1, SoftwareSystemId = 1, Name = "Monthly Subscription", RenewalPeriodInMonths = 1, RenewalPrice = 900, StartDate = DateTime.Now, IsCancelled = true},
            new Subscription {Id = 2, ClientId = 2, SoftwareSystemId = 2, Name = "Monthly Subscription", RenewalPeriodInMonths = 1, RenewalPrice = 1200, StartDate = DateTime.Now, IsCancelled = true},
            new Subscription {Id = 3, ClientId = 3, SoftwareSystemId = 3, Name = "2 Years Subscription", RenewalPeriodInMonths = 24, RenewalPrice = 8000, StartDate = DateTime.Now, IsCancelled = true}
        );
    }

}