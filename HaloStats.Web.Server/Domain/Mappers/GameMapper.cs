using HaloStats.Database.Entities;
using HaloStats.Web.Shared.Contracts.Game;
using HaloStats.Web.Shared.Contracts.Shared;

namespace HaloStats.Web.Server.Domain.Mappers;

public static class GameMapper
{
    public static GetGameResponse MapToGetGameResponse(Game game)
    {
        // Find max/min values
        var maxKills = game.Players.Max(p => p.Kills);
        var maxAssists = game.Players.Max(p => p.Assists);
        var maxScore = game.Players.Max(p => p.Score);
        var minDeaths = game.Players.Min(p => p.Deaths);

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
                Gamertag = p.Gamertag,
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
                IsHighestKills = p.Kills == maxKills,
                IsHighestAssists = p.Assists == maxAssists,
                IsHighestScore = p.Score == maxScore,
                IsLowestDeaths = p.Deaths == minDeaths,
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
