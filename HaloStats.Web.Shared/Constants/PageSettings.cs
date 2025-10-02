namespace HaloStats.Web.Shared.Constants;

public static class PageSettings
{
    public const int DefaultPlayerGamesPageSize = 100;
    

    public static class Leaderboard
    {
        public const int DefaultPageSize = 10;
        public const string SortKills = "kills";
        public const string SortKdr = "kdr";
        public const string DefaultSortBy = SortKills;
    }

    public abstract class TeammatePairs
    {
        public const string SortGamesPlayedTogether = "GamesPlayedTogether";
        public const string SortWinsTogether = "WinsTogether";
        public const string SortWinRate = "WinRate";
        public const string SortLossesTogether = "LossesTogether";
    }

    public class LeaderboardTeammatePairs : TeammatePairs
    {
        public const int DefaultPageSize = 10;
        public const string DefaultSortBy = SortWinRate;
    }

    public class PlayerTeammatePairs : TeammatePairs
    {
        public const int DefaultPageSize = 25;
        public const string DefaultSortBy = SortGamesPlayedTogether;
    }
}
