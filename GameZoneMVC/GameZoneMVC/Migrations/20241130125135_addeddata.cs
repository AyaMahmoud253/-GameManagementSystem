using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameZoneMVC.Migrations
{
    /// <inheritdoc />
    public partial class addeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Xbox");

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CategoryId", "Cover", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, "fifa2024.jpg", "A popular football game", "FIFA 2024" },
                    { 2, 2, "cod.jpg", "First-person shooter game", "Call of Duty" }
                });

            migrationBuilder.InsertData(
                table: "GameDevices",
                columns: new[] { "DeviceId", "GameId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 4, 1 },
                    { 2, 2 },
                    { 4, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameDevices",
                keyColumns: new[] { "DeviceId", "GameId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "GameDevices",
                keyColumns: new[] { "DeviceId", "GameId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "GameDevices",
                keyColumns: new[] { "DeviceId", "GameId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "GameDevices",
                keyColumns: new[] { "DeviceId", "GameId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "xbox");
        }
    }
}
