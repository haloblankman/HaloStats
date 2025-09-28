using HaloStats.Database.Entities;
using HaloStats.Web.Shared.Contracts.Dashboard;
using HaloStats.Web.Shared.Enums;

namespace HaloStats.Web.Server.Domain.Mappers;

internal static class DashboardMapper
{
    public static DashboardResponse MapToDashboardResponse(this List<Game> recentGames, int totalGames)
    {
        var recentGameList = new List<RecentGame>();

        foreach (var game in recentGames)
        {
            HaloGames gameType = game.GameEnum.MapToHaloGames();

            var recentGame = new RecentGame
            {
                GameId = game.GameId,
                GameTypeName = game.GameTypeName,
                GameType = gameType,
                IsTeamGame = game.IsTeamsEnabled,
                Teams = null,
                NonTeamGame = null
            };

            if (game.IsTeamsEnabled)
            {
                // Group players by TeamId
                var teamGroups = game.Players
                    .GroupBy(p => p.TeamId)
                    .Select(g => new
                    {
                        TeamId = g.Key,
                        Gamertags = g.Select(p => p.Gamertag).ToList(),
                        Score = g.Sum(p => p.Score)
                    })
                    .ToList();

                // Find the highest team score
                var maxScore = teamGroups.Max(t => t.Score);

                recentGame.Teams = teamGroups
                    .Select(t => new RecentTeamGame
                    {
                        TeamId = t.TeamId,
                        Gamertags = t.Gamertags,
                        IsWinner = t.Score == maxScore,
                        Score = t.Score
                    })
                    .OrderByDescending(t => t.Score)
                    .ToList();
            }
            else
            {
                // Non-team game: winner is the player with the highest score
                var maxScore = game.Players.Max(p => p.Score);
                var winner = game.Players.First(p => p.Score == maxScore);

                recentGame.NonTeamGame = new RecentNonTeamGame
                {
                    Gamertags = game.Players.Select(p => p.Gamertag).ToList(),
                    WinnerGamertag = winner.Gamertag,
                    Score = winner.Score
                };
            }

            recentGameList.Add(recentGame);
        }

        return new DashboardResponse
        {
            TotalGames = totalGames,
            RecentGames = recentGameList
        };
    }
}
