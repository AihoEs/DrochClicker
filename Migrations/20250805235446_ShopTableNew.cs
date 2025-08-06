using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drochclicker.Migrations
{
    /// <inheritdoc />
    public partial class ShopTableNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreviousUpgradeId",
                table: "Upgrades",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousUpgradeId",
                table: "Upgrades");
        }
    }
}
