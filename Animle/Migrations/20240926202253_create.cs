using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animle.Migrations
{
    /// <inheritdoc />
    public partial class create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AnimeWithEmoji",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JapaneseTitle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmojiDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Thumbnail = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MyanimeListId = table.Column<int>(type: "int", nullable: false),
                    properties = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeWithEmoji", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DailyChallenges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimeCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyChallenges", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UnathenticatedGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Fingerpring = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GameId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnathenticatedGames", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AnimeDailyChallenges",
                columns: table => new
                {
                    AnimesId = table.Column<int>(type: "int", nullable: false),
                    DailyChallengesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeDailyChallenges", x => new { x.AnimesId, x.DailyChallengesId });
                    table.ForeignKey(
                        name: "FK_AnimeDailyChallenges_AnimeWithEmoji_AnimesId",
                        column: x => x.AnimesId,
                        principalTable: "AnimeWithEmoji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeDailyChallenges_DailyChallenges_DailyChallengesId",
                        column: x => x.DailyChallengesId,
                        principalTable: "DailyChallenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameContests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimePlayed = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false),
                    ChallengeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameContests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameContests_DailyChallenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "DailyChallenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameContests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Quizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Title = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Thumbnail = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quizes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Threebythree",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Threebythree", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Threebythree_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Versus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Points = table.Column<int>(type: "int", nullable: false),
                    gameWon = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Versus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Versus_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AnimeWithEmojiQuiz",
                columns: table => new
                {
                    AnimesId = table.Column<int>(type: "int", nullable: false),
                    QuizzesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeWithEmojiQuiz", x => new { x.AnimesId, x.QuizzesId });
                    table.ForeignKey(
                        name: "FK_AnimeWithEmojiQuiz_AnimeWithEmoji_AnimesId",
                        column: x => x.AnimesId,
                        principalTable: "AnimeWithEmoji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeWithEmojiQuiz_Quizes_QuizzesId",
                        column: x => x.QuizzesId,
                        principalTable: "Quizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QuizLikes",
                columns: table => new
                {
                    quizId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizLikes", x => new { x.UserId, x.quizId });
                    table.ForeignKey(
                        name: "FK_QuizLikes_Quizes_quizId",
                        column: x => x.quizId,
                        principalTable: "Quizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AnimeThreebythree",
                columns: table => new
                {
                    AnimesId = table.Column<int>(type: "int", nullable: false),
                    ThreebythreeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeThreebythree", x => new { x.AnimesId, x.ThreebythreeId });
                    table.ForeignKey(
                        name: "FK_AnimeThreebythree_AnimeWithEmoji_AnimesId",
                        column: x => x.AnimesId,
                        principalTable: "AnimeWithEmoji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeThreebythree_Threebythree_ThreebythreeId",
                        column: x => x.ThreebythreeId,
                        principalTable: "Threebythree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "threebythreeLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ThreebythreeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_threebythreeLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_threebythreeLikes_Threebythree_ThreebythreeId",
                        column: x => x.ThreebythreeId,
                        principalTable: "Threebythree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_threebythreeLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeDailyChallenges_DailyChallengesId",
                table: "AnimeDailyChallenges",
                column: "DailyChallengesId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeThreebythree_ThreebythreeId",
                table: "AnimeThreebythree",
                column: "ThreebythreeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeWithEmojiQuiz_QuizzesId",
                table: "AnimeWithEmojiQuiz",
                column: "QuizzesId");

            migrationBuilder.CreateIndex(
                name: "IX_GameContests_ChallengeId",
                table: "GameContests",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_GameContests_UserId",
                table: "GameContests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizes_UserId",
                table: "Quizes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizLikes_quizId",
                table: "QuizLikes",
                column: "quizId");

            migrationBuilder.CreateIndex(
                name: "IX_Threebythree_UserId",
                table: "Threebythree",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_threebythreeLikes_ThreebythreeId",
                table: "threebythreeLikes",
                column: "ThreebythreeId");

            migrationBuilder.CreateIndex(
                name: "IX_threebythreeLikes_UserId",
                table: "threebythreeLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name_Email",
                table: "Users",
                columns: new[] { "Name", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Versus_UserId",
                table: "Versus",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeDailyChallenges");

            migrationBuilder.DropTable(
                name: "AnimeThreebythree");

            migrationBuilder.DropTable(
                name: "AnimeWithEmojiQuiz");

            migrationBuilder.DropTable(
                name: "GameContests");

            migrationBuilder.DropTable(
                name: "QuizLikes");

            migrationBuilder.DropTable(
                name: "threebythreeLikes");

            migrationBuilder.DropTable(
                name: "UnathenticatedGames");

            migrationBuilder.DropTable(
                name: "Versus");

            migrationBuilder.DropTable(
                name: "AnimeWithEmoji");

            migrationBuilder.DropTable(
                name: "DailyChallenges");

            migrationBuilder.DropTable(
                name: "Quizes");

            migrationBuilder.DropTable(
                name: "Threebythree");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
