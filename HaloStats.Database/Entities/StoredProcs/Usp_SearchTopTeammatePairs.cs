namespace HaloStats.Database.Entities.StoredProcs;

public class Usp_SearchTopTeammatePairs
{
    public required int Rank { get; set; }
    public required string PlayerOneGamertag { get; set; }
    public required string PlayerTwoGamertag { get; set; }
    public required int GamesPlayedTogether { get; set; }
    public required int WinsTogether { get; set; }
    public required int LossesTogether { get; set; }
    public required decimal WinRate { get; set; }
    public int TotalCount { get; set; }
}
