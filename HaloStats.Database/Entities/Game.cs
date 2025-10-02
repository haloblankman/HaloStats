using HaloStats.Database.Enums;
using UUIDNext;

namespace HaloStats.Database.Entities;
public class Game
{
    public Guid GameId { get; set; }
    public required Guid GameUniqueId { get; set; }
    public bool IsDuplicateGame { get; set; }
    public bool IsDeleted { get; set; }
    public required string GameTypeName { get; set; }
    public required bool IsMatchmaking { get; set; }
    public required bool LastMatchIncomplete { get; set; }
    public required bool IsTeamsEnabled { get; set; }
    public required uint HopperId { get; set; }
    public string? HopperName { get; set; }
    public required int PartySize { get; set; }
    public required bool HasNetworkMembersInParty { get; set; }
    public required GameEnum GameEnum { get; set; }
    public required string WhosReportingIp { get; set; }
    public DateTime ReportedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedByIp { get; set; }
    public required List<GamePlayer> Players { get; set; }
    public int TeamSize { get; set; }

    public Game()
    {
        GameId = Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
        ReportedAt = DateTime.UtcNow;
        IsDuplicateGame = false;
        IsDeleted = false;
    }

    public void Duplicate()
    {
        IsDuplicateGame = true;
    }

    public bool IsGameDuplicate(List<Game> previousGames)
    {
        return previousGames.Any(IsDuplicate);
    }

    private bool IsDuplicate(Game previous)
    {
        if (previous == null || previous.Players.Count != Players.Count)
            return false;

        // Compare each player in this game to a matching player in the previous game
        foreach (var player in Players)
        {
            var prevPlayer = previous.Players.FirstOrDefault(p =>
                p.XboxUserId == player.XboxUserId &&
                p.Kills == player.Kills &&
                p.Assists == player.Assists &&
                p.Score == player.Score &&
                p.Deaths == player.Deaths);

            if (prevPlayer == null)
                return false;
        }

        return true;
    }

    public bool IsLatestGame(List<Game> previousGames)
    {
        if (previousGames == null || previousGames.Count == 0 || Players == null)
            return true;

        int thisTotal = Players.Sum(p => p.Kills + p.Deaths + p.Assists + p.Score);
        int maxPrevTotal = previousGames
            .Where(g => g.Players != null)
            .Select(g => g.Players.Sum(p => p.Kills + p.Deaths + p.Assists + p.Score))
            .DefaultIfEmpty(0)
            .Max();

        return thisTotal > maxPrevTotal;
    }

    public void SetCalculatedFields()
    {
        CalcualtePlayerStandings();
        CalculateTeamSize();
        CalculateKilledMostGamertags();
    }

    private void CalcualtePlayerStandings()
    {
        if (Players == null || Players.Count == 0)
            return;

        var rankedPlayers = Players
            .OrderByDescending(p => p.Score)
            .ThenByDescending(p => p.Kills)
            .ThenBy(p => p.Deaths)
            .ToList();
        for (int i = 0; i < rankedPlayers.Count; i++)
        {
            rankedPlayers[i].Place = i + 1;
        }

        if (IsTeamsEnabled)
        {
            var teamScores = Players
                .GroupBy(p => p.TeamId)
                .Select(g => new
                {
                    TeamId = g.Key,
                    TotalScore = g.Sum(p => p.Score)
                })
                .OrderByDescending(t => t.TotalScore)
                .ToList();
            var winningTeam = teamScores.First();
            foreach (var player in Players)
            {
                player.IsWinner = player.TeamId == winningTeam.TeamId;
            }
        }
        else
        {
            var winner = rankedPlayers.First();
            winner.IsWinner = true;
        }
    }

    private void CalculateTeamSize()
    {
        if (Players == null || Players.Count == 0 || !IsTeamsEnabled)
        {
            TeamSize = 0;
            return;
        }

        var teamSizes = Players
            .GroupBy(p => p.TeamId)
            .Select(g => g.Count())
            .ToList();
        TeamSize = teamSizes.Max();
    }

    private void CalculateKilledMostGamertags() {         
        if (Players == null || Players.Count == 0)
            return;

        foreach (var player in Players)
        {
            if (player.KilledMostPlayerIndex != -1 && Players.Count() - 1 >= player.KilledMostPlayerIndex)
            {
                player.KilledMostGamertag = Players[player.KilledMostPlayerIndex].Gamertag;
            }
            else
            {
                player.KilledMostGamertag = null;
            }

            if (player.MostKilledByPlayerIndex != -1 && Players.Count() - 1 >= player.MostKilledByPlayerIndex)
            {
                player.MostKilledByGamertag = Players[player.MostKilledByPlayerIndex].Gamertag;
            }
            else
            {
                player.MostKilledByGamertag = null;
            }
        }
    }

    public void Delete(string ipAddress)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedByIp = ipAddress;
    }
}
