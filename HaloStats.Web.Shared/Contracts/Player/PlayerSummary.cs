namespace HaloStats.Web.Shared.Contracts.Player;

public class PlayerSummary
{
    public required int GamesPlayedCount { get; set; }
    public required int TotalWins { get; set; }
    public required int TotalKills { get; set; }
    public required int TotalAssists { get; set; }
    public required int TotalDeaths { get; set; }
    public required decimal WinPercentage { get; set; }
    public required decimal KillDeathRatio { get; set; }
    public required decimal KillAssistDeathRatio { get; set; }
}
