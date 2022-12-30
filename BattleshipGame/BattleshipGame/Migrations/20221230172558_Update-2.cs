using Microsoft.EntityFrameworkCore.Migrations;

namespace BattleshipGame.Migrations
{
    public partial class Update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_matchScores",
                table: "matchScores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_matchSaveAndReplays",
                table: "matchSaveAndReplays");

            migrationBuilder.DropColumn(
                name: "Player1CurrentShipsType",
                table: "matchSaveAndReplays");

            migrationBuilder.DropColumn(
                name: "Player2CurrentShipsType",
                table: "matchSaveAndReplays");

            migrationBuilder.RenameTable(
                name: "matchScores",
                newName: "MatchScores");

            migrationBuilder.RenameTable(
                name: "matchSaveAndReplays",
                newName: "MatchSaveAndReplays");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchScores",
                table: "MatchScores",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchSaveAndReplays",
                table: "MatchSaveAndReplays",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchScores",
                table: "MatchScores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchSaveAndReplays",
                table: "MatchSaveAndReplays");

            migrationBuilder.RenameTable(
                name: "MatchScores",
                newName: "matchScores");

            migrationBuilder.RenameTable(
                name: "MatchSaveAndReplays",
                newName: "matchSaveAndReplays");

            migrationBuilder.AddColumn<string>(
                name: "Player1CurrentShipsType",
                table: "matchSaveAndReplays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Player2CurrentShipsType",
                table: "matchSaveAndReplays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_matchScores",
                table: "matchScores",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_matchSaveAndReplays",
                table: "matchSaveAndReplays",
                column: "Id");
        }
    }
}
