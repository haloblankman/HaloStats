using HaloStats.Database.Entities;
using HaloStats.Web.Shared.Contracts.Game;
using HaloStats.Web.Shared.Contracts.Shared;

namespace HaloStats.Web.Server.Domain.Mappers;

public static class GameMapper
{
    public static GetGameResponse MapToGetGameResponse(Game game)
    {
        var response = new GetGameResponse
        {
            GameId = game.GameId,
            IsMatchmaking = game.IsMatchmaking,
            IsTeamsEnabled = game.IsTeamsEnabled,
            GameTypeName = game.GameTypeName,
            GamePlayers = game.Players.Select(p => new GamePlayerStats
            {
                Assists = p.Assists,
                ClanTag = p.ClanTag,
                Deaths = p.Deaths,
                GamerTag = p.GamerTag,
                GrenadeKills = p.KillsGrenade,
                KillDeathSpread = p.Kills - p.Deaths,
                Kills = p.Kills,
                Medals = p.TotalMedalCount,
                MeleeKills = p.KillsMelee,
                OtherKills = p.KillsOther,
                Place = p.Place,
                Score = p.Score,
                SecondsAlive = p.SecondsAlive,
                Spree = p.MostKillsInARow,
                WeaponKills = p.KillsWeapon,
                TeamId = p.TeamId,
            }).ToList(),
        };

        if (!game.IsTeamsEnabled)
        {
            return response;
        }

        var teams = game.Players
            .GroupBy(p => p.TeamId)
            .Select(g => new TeamScore
            {
                TeamId = g.Key,
                Score = g.Sum(p => p.Score),
                TeamName = GameEnumMapper.MapTeamIdToName(g.Key)
            })
            .OrderByDescending(t => t.Score)
            .ToList();

        int place = 1;
        foreach (var team in teams)
        {
            team.Place = place++;
        }
        
        var winner = teams.First();
        winner.IsWinner = true;
        
        response.TeamScores = teams;
        
        return response;
    }
}
