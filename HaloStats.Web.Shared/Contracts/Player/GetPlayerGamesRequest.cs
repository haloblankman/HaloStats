namespace HaloStats.Web.Shared.Contracts.Player;

public class GetPlayerGamesRequest
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? FilterGamertag { get; set; }
}
