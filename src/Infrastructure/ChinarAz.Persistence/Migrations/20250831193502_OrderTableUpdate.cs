using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChinarAz.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OrderTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressText",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressText",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Order");
        }
    }
}
