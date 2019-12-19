using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialDbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameSettings",
                columns: table => new
                {
                    GameSettingsId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BoardHeight = table.Column<int>(nullable: false),
                    BoardWidth = table.Column<int>(nullable: false),
                    PlayerOneName = table.Column<string>(nullable: true),
                    PlayerTwoName = table.Column<string>(nullable: true),
                    GameMode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSettings", x => x.GameSettingsId);
                });

            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    GameStateId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GameStateName = table.Column<string>(nullable: true),
                    Turn = table.Column<bool>(nullable: false),
                    BoardHeight = table.Column<int>(nullable: false),
                    BoardWidth = table.Column<int>(nullable: false),
                    BoardJson = table.Column<string>(nullable: true),
                    GameMode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStates", x => x.GameStateId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameSettings");

            migrationBuilder.DropTable(
                name: "GameStates");
        }
    }
}
