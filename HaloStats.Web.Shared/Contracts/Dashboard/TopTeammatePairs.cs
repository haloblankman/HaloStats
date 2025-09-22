namespace HaloStats.Web.Shared.Contracts.Dashboard;

public class TopTeammatePairs
{
    public required List<TeammatePair> Pairs { get; set; }
}

public class TeammatePair
{
    public required int Rank { get; set; }
    public required string PlayerOneGamerTag { get; set; }
    public required string PlayerTwoGamerTag { get; set; }
    public required int GamesPlayedTogether { get; set; }
    public required int WinsTogether { get; set; }
    public required int LossesTogether { get; set; }
    public required decimal WinRate { get; set; }
}
