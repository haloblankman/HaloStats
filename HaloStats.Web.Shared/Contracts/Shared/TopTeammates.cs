namespace HaloStats.Web.Shared.Contracts.Shared;

public class TopTeammates
{
    public required List<TopTeammateRecord> Teammates { get; set; }
}

public class TopTeammateRecord
{
    public required int Rank { get; set; }
    public required string TeammateGamertag { get; set; }
    public required int GamesPlayedTogether { get; set; }
    public required int WinsTogether { get; set; }
    public required int LossesTogether { get; set; }
    public required decimal WinRate { get; set; }
}

