using HaloStats.Database.Enums;
using HaloStats.Web.Shared.Enums;

namespace HaloStats.Web.Server.Domain.Mappers;

public static class GameEnumMapper
{
    public static GameEnum? MapToGameEnum(this HaloGames game)
    {
        return game switch
        {
            HaloGames.HaloCe => GameEnum.HaloCe,
            HaloGames.Halo2 => GameEnum.Halo2,
            HaloGames.Halo2Anniversary => GameEnum.Halo2Anniversary,
            HaloGames.Halo3 => GameEnum.Halo3,
            HaloGames.Halo3Odst => GameEnum.Halo3Odst,
            HaloGames.Halo4 => GameEnum.Halo4,
            HaloGames.HaloReach => GameEnum.HaloReach,
            HaloGames.HaloMccAll => null,
            _ => null
        };
    }

    public static HaloGames MapToHaloGames(this GameEnum gameEnum)
    {
        return gameEnum switch
        {
            GameEnum.HaloCe => HaloGames.HaloCe,
            GameEnum.Halo2 => HaloGames.Halo2,
            GameEnum.Halo2Anniversary => HaloGames.Halo2Anniversary,
            GameEnum.Halo3 => HaloGames.Halo3,
            GameEnum.Halo3Odst => HaloGames.Halo3Odst,
            GameEnum.Halo4 => HaloGames.Halo4,
            GameEnum.HaloReach => HaloGames.HaloReach,
            _ => HaloGames.HaloMccAll
        };
    }

    public static string MapTeamIdToName(int teamId)
    {
        return teamId switch
        {
            0 => "Red",
            1 => "Blue",
            2 => "Green",
            3 => "Orange",
            4 => "Brown",
            5 => "White",
            6 => "Pink",
            7 => "Gold",
            8 => "Gray",
            9 => "Purple",
            _ => "Unknown"
        };
    }
}
