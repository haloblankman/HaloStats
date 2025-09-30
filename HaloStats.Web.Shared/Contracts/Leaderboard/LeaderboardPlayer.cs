namespace HaloStats.Web.Shared.Contracts.Leaderboard;

public class LeaderboardPlayer
{
    public required int Rank { get; set; }
    public required string Gamertag { get; set; }
    public required int Kills { get; set; }
    public required int Deaths { get; set; }
    public required decimal KillDeathRatio { get; set; }
}
