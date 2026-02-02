using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RevenueRecognitionSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SoftwareSystemAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    UpfrontPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MonthlySubscriptionPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    IdClient = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoftwareSystems_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "ClientType", "Email", "FirstName", "IsDeleted", "LastName", "PESEL", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Warsaw, ul. Centralna 10", "Individual", "jan.kowalski@example.com", "Jan", false, "Kowalski", "90010112345", "123456789" },
                    { 2, "Krakow, ul. Zielona 5", "Individual", "anna.nowak@example.com", "Anna", false, "Nowak", "85050567890", "987654321" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "ClientType", "CompanyName", "Email", "KRSNumber", "PhoneNumber" },
                values: new object[,]
                {
                    { 3, "Gdansk, ul. Portowa 12", "Company", "TechSoft Ltd.", "info@techsoft.com", "0000123456", "111222333" },
                    { 4, "Poznan, ul. Edukacyjna 7", "Company", "EduCorp Sp. z o.o.", "contact@educorp.com", "0000654321", "444555666" }
                });

            migrationBuilder.InsertData(
                table: "SoftwareSystems",
                columns: new[] { "Id", "Category", "CurrentVersion", "Description", "IdClient", "MonthlySubscriptionPrice", "Name", "UpfrontPrice" },
                values: new object[,]
                {
                    { 1, 1, "1.2.3", "Financial software", 1, 29.99m, "FinTrack", 499.99m },
                    { 2, 2, "3.0.0", "Education platform", 4, 19.99m, "LearnMate", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareSystems_IdClient",
                table: "SoftwareSystems",
                column: "IdClient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoftwareSystems");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
