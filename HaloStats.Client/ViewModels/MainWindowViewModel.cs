using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HaloStats.Client.Domain.Mappers;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net.Http;
using System.Windows;

namespace HaloStats.Client.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly FileWatcher _fileWatcher = new();

    [ObservableProperty]
    private int selectedGameIndex;

    public ObservableCollection<Game> Games { get; } = new();

    public MainWindowViewModel()
    {
        CloseTabCommand = new RelayCommand<Game>(CloseTab);
        DeleteGameCommand = new AsyncRelayCommand<Game>(DeleteGameAsync);
        _fileWatcher.ReportParsed += OnReportParsed;
        _fileWatcher.WatchFolder();
    }

    public IRelayCommand<Game> CloseTabCommand { get; }
    public IAsyncRelayCommand<Game> DeleteGameCommand { get; }

    private async void OnReportParsed(MultiplayerCarnageReport report)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var duplicateGame = Games.FirstOrDefault(g => g.GameUniqueId == report.GameUniqueId.Value);
            if (duplicateGame is not null)
            {
                Games.Remove(duplicateGame);
            }

            var game = MapGame(report);
            Games.Add(game);
            SelectedGameIndex = Games.Count - 1;
        });

        await PostCarnageReportAsync(report);
    }

    private async Task DeleteGameAsync(Game? game)
    {
        if (game == null) return;

        var result = MessageBox.Show(
            "Are you sure you want to delete this game?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
            return;

        string apiUrl = ConfigurationManager.AppSettings["ApiUrl"]!;
        string deleteUrl = $"{apiUrl.TrimEnd('/')}/api/CarnageReport/{game.GameUniqueId}";

        using var httpClient = new HttpClient();
        try
        {
            var response = await httpClient.DeleteAsync(deleteUrl);
            response.EnsureSuccessStatusCode();

            Application.Current.Dispatcher.Invoke(() => CloseTab(game));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to delete game: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task PostCarnageReportAsync(MultiplayerCarnageReport report)
    {
        using var httpClient = new HttpClient();
        string apiUrl = ConfigurationManager.AppSettings["ApiUrl"]!;
        string pcrUrl = $"{apiUrl.TrimEnd('/')}/api/CarnageReport";
        var request = MultiplayerCarnageReportMapper.MapToPostRequest(report);
        var json = System.Text.Json.JsonSerializer.Serialize(request);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        const int maxAttempts = 3;
        const int delayMilliseconds = 5000;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                var response = await httpClient.PostAsync(pcrUrl, content);
                response.EnsureSuccessStatusCode();
                return;
            }
            catch (Exception) when (attempt < maxAttempts)
            {
                await Task.Delay(delayMilliseconds);
            }
            catch
            {
            }
        }
    }

    private Game MapGame(MultiplayerCarnageReport report)
    {
        var redTeamScore = report.Players.Where(p => p.mTeamId == 0).Sum(p => p.Score);
        var blueTeamScore = report.Players.Where(p => p.mTeamId == 1).Sum(p => p.Score);
        var isRedTeamVictory = redTeamScore > blueTeamScore;
        // Map to PlayerScore list first
        var playerScores = report.Players.Select(p => new PlayerScore
        {
            TeamId = p.mTeamId,
            Gamertag = p.mGamertagText,
            Kills = p.mKills,
            Assists = p.mAssists,
            Deaths = p.mDeaths,
            Score = p.Score
        }).ToList();

        // Calculate max/min values
        var maxScore = playerScores.Max(ps => ps.Score);
        var maxAssists = playerScores.Max(ps => ps.Assists);
        var minDeaths = playerScores.Min(ps => ps.Deaths);
        var maxKills = playerScores.Max(ps => ps.Kills);

        // Set the flags
        foreach (var ps in playerScores)
        {
            ps.HasHighestScore = ps.Score == maxScore;
            ps.HasHighestAssists = ps.Assists == maxAssists;
            ps.HasLowestDeaths = ps.Deaths == minDeaths;
            ps.HasHighestKills = ps.Kills == maxKills;
        }

        // Order by score descending and assign ranks (handling ties)
        var ordered = playerScores
            .OrderByDescending(ps => ps.Score)
            .ThenByDescending(ps => ps.Kills + ps.Assists - ps.Deaths) // Secondary sort by K-D difference
            .ToList();

        for (int i = 0; i < ordered.Count; i++)
        {
            var ps = ordered[i];
            ps.Place = i + 1;
        }

        // Now assign the ranks back to the original list (if needed)
        var game = new Game
        {
            GameUniqueId = report.GameUniqueId.Value,
            GameTypeName = report.GameTypeName.Value,
            RedTeamScore = redTeamScore,
            BlueTeamScore = blueTeamScore,
            IsRedTeamVictory = isRedTeamVictory,
            PlayerScores = playerScores
        };

        return game;
    }

    private void CloseTab(Game? game)
    {
        if (game != null)
        {
            int index = Games.IndexOf(game);
            Games.Remove(game);
            if (SelectedGameIndex >= Games.Count)
                SelectedGameIndex = Games.Count - 1;
            else if (SelectedGameIndex > index)
                SelectedGameIndex--;
        }
    }
}