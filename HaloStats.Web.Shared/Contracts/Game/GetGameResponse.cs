using HaloStats.Web.Shared.Contracts.Shared;

namespace HaloStats.Web.Shared.Contracts.Game;

public class GetGameResponse
{
    public required Guid GameId { get; set; }
    public required bool IsMatchmaking { get; set; }
    public required string GameTypeName { get; set; }
    public required List<GamePlayerStats> GamePlayers { get; set; }
    public required bool IsTeamsEnabled { get; set; }
    public List<TeamScore> TeamScores { get; set; } = new List<TeamScore>();
}

public class TeamScore
{
    public required int TeamId { get; set; }
    public required string TeamName { get; set; }
    public required int Score { get; set; }
    public int Place { get; set; }
    public bool IsWinner { get; set; }
}
