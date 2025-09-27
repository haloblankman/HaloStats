namespace HaloStats.Web.Shared.Contracts.Player;

public class GetPlayerGamesRequest
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public GetPlayerGamesFilters? Filters { get; set; }
}

public class GetPlayerGamesFilters
{
    public string? Gamertag { get; set; }
}
