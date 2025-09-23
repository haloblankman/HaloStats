using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaloStats.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    GameUniqueId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDuplicateGame = table.Column<bool>(type: "boolean", nullable: false),
                    GameTypeName = table.Column<string>(type: "text", nullable: false),
                    IsMatchmaking = table.Column<bool>(type: "boolean", nullable: false),
                    LastMatchIncomplete = table.Column<bool>(type: "boolean", nullable: false),
                    IsTeamsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    HopperId = table.Column<long>(type: "bigint", nullable: false),
                    HopperName = table.Column<string>(type: "text", nullable: true),
                    PartySize = table.Column<int>(type: "integer", nullable: false),
                    HasNetworkMembersInParty = table.Column<bool>(type: "boolean", nullable: false),
                    GameEnum = table.Column<int>(type: "integer", nullable: false),
                    WhosReportingIp = table.Column<string>(type: "text", nullable: false),
                    ReportedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                });

            migrationBuilder.CreateTable(
                name: "GamePlayers",
                columns: table => new
                {
                    GamePlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    XboxUserId = table.Column<string>(type: "text", nullable: false),
                    IsGuest = table.Column<bool>(type: "boolean", nullable: false),
                    IsWinner = table.Column<bool>(type: "boolean", nullable: false),
                    Place = table.Column<int>(type: "integer", nullable: false),
                    GameMode = table.Column<int>(type: "integer", nullable: false),
                    GamerTag = table.Column<string>(type: "text", nullable: false),
                    ClanTag = table.Column<string>(type: "text", nullable: false),
                    EmblemTexture0 = table.Column<int>(type: "integer", nullable: false),
                    EmblemTexture1 = table.Column<int>(type: "integer", nullable: false),
                    EmblemColor0 = table.Column<int>(type: "integer", nullable: false),
                    EmblemColor1 = table.Column<int>(type: "integer", nullable: false),
                    Nameplate = table.Column<int>(type: "integer", nullable: false),
                    Avatar = table.Column<int>(type: "integer", nullable: false),
                    ServiceId = table.Column<string>(type: "text", nullable: false),
                    TeamId = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    Standing = table.Column<int>(type: "integer", nullable: false),
                    TotalMedalCount = table.Column<int>(type: "integer", nullable: false),
                    Kills = table.Column<int>(type: "integer", nullable: false),
                    Deaths = table.Column<int>(type: "integer", nullable: false),
                    Assists = table.Column<int>(type: "integer", nullable: false),
                    Betrayals = table.Column<int>(type: "integer", nullable: false),
                    Suicides = table.Column<int>(type: "integer", nullable: false),
                    MostKillsInARow = table.Column<int>(type: "integer", nullable: false),
                    SecondsAlive = table.Column<int>(type: "integer", nullable: false),
                    KillsWeapon = table.Column<int>(type: "integer", nullable: false),
                    KillsGrenade = table.Column<int>(type: "integer", nullable: false),
                    KillsMelee = table.Column<int>(type: "integer", nullable: false),
                    KillsOther = table.Column<int>(type: "integer", nullable: false),
                    CompletedGame = table.Column<int>(type: "integer", nullable: false),
                    SecondsPlayed = table.Column<int>(type: "integer", nullable: false),
                    KilledMostPlayerIndex = table.Column<int>(type: "integer", nullable: false),
                    KilledMostPlayerCount = table.Column<int>(type: "integer", nullable: false),
                    MostKilledByPlayerIndex = table.Column<int>(type: "integer", nullable: false),
                    MostKilledByPlayerCount = table.Column<int>(type: "integer", nullable: false),
                    MostUsedWeapon = table.Column<int>(type: "integer", nullable: false),
                    MostUsedWeaponCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayers", x => x.GamePlayerId);
                    table.ForeignKey(
                        name: "FK_GamePlayers_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamePlayerCustomStats",
                columns: table => new
                {
                    GamePlayerCustomStatId = table.Column<Guid>(type: "uuid", nullable: false),
                    GamePlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatName = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayerCustomStats", x => x.GamePlayerCustomStatId);
                    table.ForeignKey(
                        name: "FK_GamePlayerCustomStats_GamePlayers_GamePlayerId",
                        column: x => x.GamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "GamePlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamePlayerMedals",
                columns: table => new
                {
                    GamePlayerMedalId = table.Column<Guid>(type: "uuid", nullable: false),
                    GamePlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    MedalId = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayerMedals", x => x.GamePlayerMedalId);
                    table.ForeignKey(
                        name: "FK_GamePlayerMedals_GamePlayers_GamePlayerId",
                        column: x => x.GamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "GamePlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayerCustomStats_GamePlayerId",
                table: "GamePlayerCustomStats",
                column: "GamePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayerMedals_GamePlayerId",
                table: "GamePlayerMedals",
                column: "GamePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_GameId",
                table: "GamePlayers",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_GamerTag",
                table: "GamePlayers",
                column: "GamerTag");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameUniqueId",
                table: "Games",
                column: "GameUniqueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamePlayerCustomStats");

            migrationBuilder.DropTable(
                name: "GamePlayerMedals");

            migrationBuilder.DropTable(
                name: "GamePlayers");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
