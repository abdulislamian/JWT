using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JWTAuthentication.Migrations
{
    /// <inheritdoc />
    public partial class SeedSomeRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7eb3cc43-4661-47c7-ab04-c4e2b2693e56", "7eb3cc43-4661-47c7-ab04-c4e2b2693e56", "Admin", "ADMIN" },
                    { "bd1c7750-7cf5-4a51-9e0e-35181826d11d", "bd1c7750-7cf5-4a51-9e0e-35181826d11d", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7eb3cc43-4661-47c7-ab04-c4e2b2693e56");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd1c7750-7cf5-4a51-9e0e-35181826d11d");
        }
    }
}
