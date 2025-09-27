namespace HaloStats.Database.Entities.StoredProcs;

public class Usp_GetTopOpponentPairs
{
    public required string OpponentGamertag { get; set; }
    public required int GamesPlayedAgainst { get; set; }
    public required int WinsAgainst { get; set; }
    public required int LossesAgainst { get; set; }
    public required decimal WinRate { get; set; }
}
