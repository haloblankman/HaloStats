using System.Windows.Media;

namespace HaloStats.Client.ViewModels;
public class Game
{
    public Guid Id { get; set; }
    public Guid GameUniqueId { get; set; }
    public bool LastMatchIncomplete { get; set; }
    public bool IsTeamsEnabled { get; set; }
    public string GameTypeName { get; set; }
    public int RedTeamScore { get; set; }
    public int BlueTeamScore { get; set; }
    public bool IsRedTeamVictory { get; set; }
    public List<PlayerScore> PlayerScores { get; set; }
    public List<TeamScoreViewModel> OrderedTeams
    {
        get
        {
            var red = new TeamScoreViewModel
            {
                TeamName = "Red Team",
                Score = RedTeamScore,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff6666"))
            };
            var blue = new TeamScoreViewModel
            {
                TeamName = "Blue Team",
                Score = BlueTeamScore,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#66ccff"))
            };
            if (IsRedTeamVictory)
                return new List<TeamScoreViewModel> { red, blue };
            else
                return new List<TeamScoreViewModel> { blue, red };
        }
    }
}

public class TeamScoreViewModel
{
    public string TeamName { get; set; }
    public int Score { get; set; }
    public Brush Foreground { get; set; }
}

public class  PlayerScore
{
    public Guid GameId { get; set; }
    public int Place { get; set; }
    public int PlayerIndex { get; set; }
    public string XboxUserId { get; set; }
    public bool IsGuest { get; set; }
    public string Gamertag { get; set; }
    public string ClanTag { get; set; }
    public int TeamId { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
    public int Score { get; set; }

    public bool HasHighestScore { get; set; }
    public bool HasHighestKills { get; set; }
    public bool HasHighestAssists { get; set; }
    public bool HasLowestDeaths { get; set; }

    public int Betrayls { get; set; }
    public int MostKillsInARow { get; set; }
    public int Suicides { get; set; }
    public int SecondsAlive { get; set; }
    public int SecondsPlayed { get; set; }
    public int KilledMostPlayerIndex { get; set; }
    public int KilledMostPlayerCount { get; set; }
    public int MostKilledByPlayerIndex { get; set; }
    public int MostKilledByPlayerCount { get; set; }
    public int MostUsedWeapon { get; set; }
    public int MostUsedWeaponCount { get; set; }
    public List<string> Medals { get; set; }
}
