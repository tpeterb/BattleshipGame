using Microsoft.EntityFrameworkCore.Migrations;

namespace BattleshipGame.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "matchSaveAndReplays",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerName1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerName2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfTurns = table.Column<int>(type: "int", nullable: false),
                    Player1Guesses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Player2Guesses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Player1CurrentShips = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Player2CurrentShips = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Player1CurrentShipsType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Player2CurrentShipsType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayerToStart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_matchSaveAndReplays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "matchScores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerName1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerName2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfTurns = table.Column<int>(type: "int", nullable: false),
                    Player1Hits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Player2Hits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WinnerPlayerName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_matchScores", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "matchSaveAndReplays");

            migrationBuilder.DropTable(
                name: "matchScores");
        }
    }
}
