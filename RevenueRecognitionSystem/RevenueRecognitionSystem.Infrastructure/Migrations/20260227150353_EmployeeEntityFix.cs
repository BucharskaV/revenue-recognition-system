using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RevenueRecognitionSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeEntityFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    ClientType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KRSNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PESEL = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExp = table.Column<DateTime>(type: "datetime", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CurrentVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    OneYearLicensePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    SoftwareSystemId = table.Column<int>(type: "int", nullable: false),
                    SoftwareVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdateYears = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_SoftwareSystems_SoftwareSystemId",
                        column: x => x.SoftwareSystemId,
                        principalTable: "SoftwareSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Target = table.Column<int>(type: "int", nullable: false),
                    SoftwareSystemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discounts_SoftwareSystems_SoftwareSystemId",
                        column: x => x.SoftwareSystemId,
                        principalTable: "SoftwareSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    SoftwareSystemId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RenewalPeriodInMonths = table.Column<int>(type: "int", nullable: false),
                    RenewalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_SoftwareSystems_SoftwareSystemId",
                        column: x => x.SoftwareSystemId,
                        principalTable: "SoftwareSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionPayments_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "ClientType", "CompanyName", "Email", "KRSNumber", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Gdansk 1", "Company", "TechSoft", "c1@comp.pl", "0000000001", "111111111" },
                    { 2, "Gdansk 2", "Company", "EduCorp", "c2@comp.pl", "0000000002", "111111112" },
                    { 3, "Gdynia 3", "Company", "FinGroup", "c3@comp.pl", "0000000003", "111111113" },
                    { 4, "Warsaw 4", "Company", "MedTech", "c4@comp.pl", "0000000004", "111111114" },
                    { 5, "Poznan 5", "Company", "BuildIT", "c5@comp.pl", "0000000005", "111111115" },
                    { 6, "Krakow 6", "Company", "LogiSoft", "c6@comp.pl", "0000000006", "111111116" },
                    { 7, "Lodz 7", "Company", "HealthPlus", "c7@comp.pl", "0000000007", "111111117" },
                    { 8, "Wroclaw 8", "Company", "CloudNet", "c8@comp.pl", "0000000008", "111111118" },
                    { 9, "Sopot 9", "Company", "RetailPro", "c9@comp.pl", "0000000009", "111111119" },
                    { 10, "Katowice 10", "Company", "AutoSys", "c10@comp.pl", "0000000010", "111111120" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "ClientType", "Email", "FirstName", "IsDeleted", "LastName", "PESEL", "PhoneNumber" },
                values: new object[,]
                {
                    { 11, "Warsaw A", "Individual", "i1@mail.pl", "Jan", false, "Kowalski", "90010100001", "222222221" },
                    { 12, "Warsaw B", "Individual", "i2@mail.pl", "Anna", false, "Nowak", "90010100002", "222222222" },
                    { 13, "Krakow C", "Individual", "i3@mail.pl", "Piotr", false, "Zielinski", "90010100003", "222222223" },
                    { 14, "Lodz D", "Individual", "i4@mail.pl", "Maria", false, "Wojcik", "90010100004", "222222224" },
                    { 15, "Gdansk E", "Individual", "i5@mail.pl", "Tomasz", false, "Lewandowski", "90010100005", "222222225" },
                    { 16, "Poznan F", "Individual", "i6@mail.pl", "Kasia", false, "Kaminska", "90010100006", "222222226" },
                    { 17, "Wroclaw G", "Individual", "i7@mail.pl", "Marek", false, "Kaczmarek", "90010100007", "222222227" },
                    { 18, "Szczecin H", "Individual", "i8@mail.pl", "Ewa", false, "Mazur", "90010100008", "222222228" },
                    { 19, "Lublin I", "Individual", "i9@mail.pl", "Adam", false, "Krol", "90010100009", "222222229" },
                    { 20, "Rzeszow J", "Individual", "i10@mail.pl", "Olga", false, "Dabrowska", "90010100010", "222222230" }
                });

            migrationBuilder.InsertData(
                table: "SoftwareSystems",
                columns: new[] { "Id", "Category", "CurrentVersion", "Description", "Name", "OneYearLicensePrice" },
                values: new object[,]
                {
                    { 1, 1, "1.0", "Finance System", "FinTrack", 5000m },
                    { 2, 2, "2.1", "Education Platform", "EduPro", 7000m },
                    { 3, 3, "3.2", "Medical System", "HealthCareX", 9000m },
                    { 4, 4, "1.4", "Construction Management", "BuildMaster", 8000m },
                    { 5, 4, "5.0", "Retail Software", "RetailSuite", 6500m },
                    { 6, 4, "4.0", "Cloud Platform", "CloudBase", 12000m },
                    { 7, 4, "2.5", "Logistics System", "LogiTrack", 7500m },
                    { 8, 4, "3.1", "Automotive Software", "AutoManager", 6000m },
                    { 9, 4, "6.2", "HR Management", "HRPro", 5500m },
                    { 10, 4, "1.8", "Security System", "SecureIT", 10000m }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "ClientId", "CreatedAt", "EndDate", "IsCancelled", "IsPaid", "Price", "SoftwareSystemId", "SoftwareVersion", "StartDate", "UpdateYears" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 2, 27, 16, 3, 52, 930, DateTimeKind.Local).AddTicks(1800), new DateTime(2026, 3, 9, 16, 3, 52, 930, DateTimeKind.Local).AddTicks(1867), false, true, 4500m, 1, "1.0", new DateTime(2026, 2, 27, 16, 3, 52, 930, DateTimeKind.Local).AddTicks(1863), 1 },
                    { 2, 2, new DateTime(2026, 2, 27, 16, 3, 52, 930, DateTimeKind.Local).AddTicks(1876), new DateTime(2046, 2, 27, 16, 3, 52, 930, DateTimeKind.Local).AddTicks(1880), false, true, 6000m, 2, "2.1", new DateTime(2026, 2, 27, 16, 3, 52, 930, DateTimeKind.Local).AddTicks(1878), 2 },
                    { 3, 3, new DateTime(2026, 2, 27, 16, 3, 52, 930, DateTimeKind.Local).AddTicks(1889), new DateTime(2036, 2, 27, 16, 3, 52, 930, DateTimeKind.Local).AddTicks(1892), false, true, 8500m, 3, "3.2", new DateTime(2026, 2, 27, 16, 3, 52, 930, DateTimeKind.Local).AddTicks(1891), 1 }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "EndDate", "Name", "Percentage", "SoftwareSystemId", "StartDate", "Target" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "New Year", 10m, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 2, new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spring Sale", 15m, 2, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Health Promo", 5m, 3, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 4, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Summer Deal", 20m, 4, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 5, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Autumn Deal", 12m, 5, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 6, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cloud Discount", 18m, 6, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 7, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Logistics Promo", 8m, 7, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 8, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Auto Deal", 7m, 8, new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 9, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR Promo", 6m, 9, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 10, new DateTime(2026, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Security Week", 25m, 10, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "Id", "ClientId", "IsCancelled", "Name", "RenewalPeriodInMonths", "RenewalPrice", "SoftwareSystemId", "StartDate" },
                values: new object[,]
                {
                    { 1, 1, true, "Monthly Subscription", 1, 900m, 1, new DateTime(2026, 2, 27, 16, 3, 52, 930, DateTimeKind.Local).AddTicks(1924) },
                    { 2, 2, true, "Monthly Subscription", 1, 1200m, 2, new DateTime(2026, 2, 27, 16, 3, 52, 930, DateTimeKind.Local).AddTicks(1929) },
                    { 3, 3, true, "2 Years Subscription", 24, 8000m, 3, new DateTime(2026, 2, 27, 16, 3, 52, 930, DateTimeKind.Local).AddTicks(1934) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ClientId",
                table: "Contracts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_SoftwareSystemId",
                table: "Contracts",
                column: "SoftwareSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_SoftwareSystemId",
                table: "Discounts",
                column: "SoftwareSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ContractId",
                table: "Payments",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPayments_SubscriptionId",
                table: "SubscriptionPayments",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ClientId",
                table: "Subscriptions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SoftwareSystemId",
                table: "Subscriptions",
                column: "SoftwareSystemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "SubscriptionPayments");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "SoftwareSystems");
        }
    }
}
