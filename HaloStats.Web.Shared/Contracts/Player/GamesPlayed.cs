namespace HaloStats.Web.Shared.Contracts.Player;

public class GamesPlayed
{
    public required List<GamePlayed> Games { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool IsNextPage { get; set; }
}

public class GamePlayed
{
    public required Guid GameId { get; set; }
    public required DateTime ReportedAt { get; set; }
    public int Score { get; set; }
    public int Kills { get; set; }
    public int Assists { get; set; }
    public int Deaths { get; set; }
}
