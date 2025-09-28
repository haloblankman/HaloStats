using HaloStats.Web.Shared.Enums;

namespace HaloStats.Web.Client.Infrastructure.Extensions;

public static class HaloTeamColorsMapper
{
    public static string ToColor(this int teamId)
    {
        return ((HaloTeamColors)teamId).ToString();
    }
}
