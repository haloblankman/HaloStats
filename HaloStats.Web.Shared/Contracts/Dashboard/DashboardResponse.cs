using HaloStats.Web.Shared.Enums;

namespace HaloStats.Web.Shared.Contracts.Dashboard;

public class DashboardResponse
{
    public required int TotalGames { get; set; }
    public required List<RecentGame> RecentGames { get; set; }
}

public class RecentGame
{
    public required Guid GameId { get; set; }
    public required string GameTypeName { get; set; }
    public required HaloGames GameType { get; set; }
    public required bool IsTeamGame { get; set; }
    public List<RecentTeamGame>? Teams { get; set; }
    public RecentNonTeamGame? NonTeamGame { get; set; }
}

public class RecentTeamGame
{
    public required int TeamId { get; set; }
    public required List<string> Gamertags { get; set; }
    public required bool IsWinner { get; set; }
    public required int Score { get; set; }
}

public class RecentNonTeamGame
{
    public required List<string> Gamertags { get; set; }
    public required string WinnerGamertag { get; set; }
    public required int Score { get; set; }
}
