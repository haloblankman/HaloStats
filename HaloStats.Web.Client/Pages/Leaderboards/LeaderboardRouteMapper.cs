using HaloStats.Web.Shared.Enums;

namespace HaloStats.Web.Client.Pages.Leaderboards;

public static class LeaderboardRouteMapper
{
    public static HaloGames? MapRouteToGameEnum(string? route) 
    {
        return route?.ToLower() switch
        {
            "halo-mcc-all" => HaloGames.HaloMccAll,
            "halo-ce" => HaloGames.HaloCe,
            "halo-2" => HaloGames.Halo2,
            "halo-2-anniversary" => HaloGames.Halo2Anniversary,
            "halo-3" => HaloGames.Halo3,
            "halo-3-odst" => HaloGames.Halo3Odst,
            "halo-4" => HaloGames.Halo4,
            "halo-reach" => HaloGames.HaloReach,
            _ => null
        };
    }

    public static string MapGameEnumToDescription(HaloGames game)
    {
        return game switch
        {
            HaloGames.HaloMccAll => "Halo: The Master Chief Collection (All Games)",
            HaloGames.HaloCe => "Halo: Combat Evolved",
            HaloGames.Halo2 => "Halo 2",
            HaloGames.Halo2Anniversary => "Halo 2: Anniversary",
            HaloGames.Halo3 => "Halo 3",
            HaloGames.Halo3Odst => "Halo 3: ODST",
            HaloGames.Halo4 => "Halo 4",
            HaloGames.HaloReach => "Halo: Reach",
            _ => "Unknown Game"
        };
    }
}
