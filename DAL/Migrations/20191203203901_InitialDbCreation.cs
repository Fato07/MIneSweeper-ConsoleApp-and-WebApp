using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialDbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameSave",
                columns: table => new
                {
                    GameSaveId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SaveName = table.Column<string>(nullable: false),
                    BoardHeight = table.Column<int>(nullable: false),
                    BoardWidth = table.Column<int>(nullable: false),
                    NumberOfMines = table.Column<int>(nullable: false),
                    BoardState = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSave", x => x.GameSaveId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameSave");
        }
    }
}
