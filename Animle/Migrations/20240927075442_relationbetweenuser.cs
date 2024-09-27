using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animle.Migrations
{
    /// <inheritdoc />
    public partial class relationbetweenuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserGuessGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserGuessGames_UserId",
                table: "UserGuessGames",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGuessGames_Users_UserId",
                table: "UserGuessGames",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGuessGames_Users_UserId",
                table: "UserGuessGames");

            migrationBuilder.DropIndex(
                name: "IX_UserGuessGames_UserId",
                table: "UserGuessGames");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserGuessGames");
        }
    }
}
