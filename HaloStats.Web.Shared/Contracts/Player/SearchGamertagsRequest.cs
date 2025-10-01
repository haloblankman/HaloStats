using HaloStats.Web.Shared.Enums;

namespace HaloStats.Web.Shared.Contracts.Player;

public class SearchGamertagsRequest
{
    public HaloGames? Game { get; set; }
    public string? PlayedWithGamertag { get; set; }
}
