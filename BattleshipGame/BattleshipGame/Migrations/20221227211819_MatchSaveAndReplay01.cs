using Microsoft.EntityFrameworkCore.Migrations;

namespace BattleshipGame.Migrations
{
    public partial class MatchSaveAndReplay01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Player1OriginalShips",
                table: "matchSaveAndReplays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Player2OriginalShips",
                table: "matchSaveAndReplays",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Player1OriginalShips",
                table: "matchSaveAndReplays");

            migrationBuilder.DropColumn(
                name: "Player2OriginalShips",
                table: "matchSaveAndReplays");
        }
    }
}
