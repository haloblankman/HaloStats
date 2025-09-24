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
    public required string GameTypeName { get; set; }
    public required int Score { get; set; }
    public required int Kills { get; set; }
    public required int Assists { get; set; }
    public required int Deaths { get; set; }
    public bool IsMatchmaking { get; set; }
}
