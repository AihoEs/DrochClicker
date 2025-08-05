using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Drochclicker.Migrations
{
    /// <inheritdoc />
    public partial class ShopTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Upgrades",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EffectType = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upgrades", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "UserUpgrades",
                columns: table => new
                {
                    UpgradeId = table.Column<int>(type: "integer", nullable: false),
                    UserLogin = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    ShopItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUpgrades", x => new { x.UserLogin, x.UpgradeId });
                    table.ForeignKey(
                        name: "FK_UserUpgrades_DataBase_UserLogin",
                        column: x => x.UserLogin,
                        principalTable: "DataBase",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUpgrades_Upgrades_UpgradeId",
                        column: x => x.UpgradeId,
                        principalTable: "Upgrades",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserUpgrades_UpgradeId",
                table: "UserUpgrades",
                column: "UpgradeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserUpgrades");

            migrationBuilder.DropTable(
                name: "Upgrades");
        }
    }
}
