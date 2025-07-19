using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelAgency.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyUserHotels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UsersHotels",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7699db7d-964f-4782-8209-d76562e0fece",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "951f241a-7e8a-4098-bf4f-50bc94b0dbe8", "AQAAAAIAAYagAAAAEN6arHv3mLhPJ9DpZjGOOxeyTwCg8o93FJFxJN9IlziAYiVkLa6ld8+seWx8huqTeA==", "b399946a-c7bc-4cbf-af20-e4bbd8d6b7ad" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UsersHotels");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7699db7d-964f-4782-8209-d76562e0fece",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8b1eb363-254e-4dd1-8127-969401434c1d", "AQAAAAIAAYagAAAAEF0HQhYlNwTXmhOS9Bsm7RRb7fdishhl8lDR/qwl7gkhzAu0AFOlGKO7dXrW0CRfdQ==", "97133ae5-a59e-40c3-bfae-ff4cd79b5def" });
        }
    }
}
