namespace HaloStats.Web.Shared.Contracts.Shared;

public class TopOpponents
{
    public required List<TopOpponentRecord> Opponents { get; set; }
}

public class TopOpponentRecord
{
    public required int Rank { get; set; }
    public required string OpponentGamertag { get; set; }
    public required int GamesPlayedAgainst { get; set; }
    public required int WinsAgainst { get; set; }
    public required int LossesAgainst { get; set; }
    public required decimal WinRate { get; set; }
}
