namespace HaloStats.Web.Shared.Contracts.Leaderboard;

public class LeaderboardTeammatePair
{
    public required int Rank { get; set; }
    public required string PlayerOneGamertag { get; set; }
    public required string PlayerTwoGamertag { get; set; }
    public required int GamesPlayedTogether { get; set; }
    public required int WinsTogether { get; set; }
    public required int LossesTogether { get; set; }
    public required decimal WinRate { get; set; }
}
