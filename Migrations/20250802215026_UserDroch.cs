using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drochclicker.Migrations
{
    /// <inheritdoc />
    public partial class UserDroch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataBase",
                columns: table => new
                {
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    ClickCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataBase", x => x.Login);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataBase");
        }
    }
}
