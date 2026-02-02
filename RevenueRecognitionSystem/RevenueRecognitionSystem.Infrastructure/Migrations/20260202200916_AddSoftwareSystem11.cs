using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RevenueRecognitionSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftwareSystem11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "ClientType", "Email", "FirstName", "IsDeleted", "LastName", "PESEL", "PhoneNumber" },
                values: new object[,]
                {
                    { 10, "Warsaw, ul. Centralna 10", "Individual", "jan.kowalski@example.com", "Jan", false, "Kowalski", "90010112345", "123456789" },
                    { 20, "Krakow, ul. Zielona 5", "Individual", "anna.nowak@example.com", "Anna", false, "Nowak", "85050567890", "987654321" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "ClientType", "Email", "FirstName", "IsDeleted", "LastName", "PESEL", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Warsaw, ul. Centralna 10", "Individual", "jan.kowalski@example.com", "Jan", false, "Kowalski", "90010112345", "123456789" },
                    { 2, "Krakow, ul. Zielona 5", "Individual", "anna.nowak@example.com", "Anna", false, "Nowak", "85050567890", "987654321" }
                });
        }
    }
}
