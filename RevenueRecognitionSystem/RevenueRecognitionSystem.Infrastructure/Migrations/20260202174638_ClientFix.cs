using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevenueRecognitionSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ClientFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Clients",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Clients");
        }
    }
}
