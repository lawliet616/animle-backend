using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animle.Migrations
{
    /// <inheritdoc />
    public partial class id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<int>(
                name: "DailyChallengeId",
                table: "GameContests",
                nullable: true
             );


            migrationBuilder.AddForeignKey(
                name: "FK_GameContests_DailyChallenges_DailyChallengeId",
                table: "GameContests",
                column: "DailyChallengeId",
                principalTable: "DailyChallenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameContests_DailyChallenges_DailyChallengeId",
                table: "GameContests");

            migrationBuilder.RenameColumn(
                name: "DailyChallengeId",
                table: "GameContests",
                newName: "ChallengeId");

            migrationBuilder.RenameIndex(
                name: "IX_GameContests_DailyChallengeId",
                table: "GameContests",
                newName: "IX_GameContests_ChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameContests_DailyChallenges_ChallengeId",
                table: "GameContests",
                column: "ChallengeId",
                principalTable: "DailyChallenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
