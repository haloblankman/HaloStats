using HaloStats.Client.Framework;
using HaloStats.Domain.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace HaloStats.Client;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private FileWatcher _fileWatcher = new FileWatcher();
    private int _selectedGameIndex;
    public ObservableCollection<Game> Games { get; } = new();
    public ICommand CloseTabCommand { get; }

    public int SelectedGameIndex
    {
        get => _selectedGameIndex;
        set
        {
            if (_selectedGameIndex != value)
            {
                _selectedGameIndex = value;
                OnPropertyChanged(nameof(SelectedGameIndex));
            }
        }
    }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        CloseTabCommand = new RelayCommand<Game>(CloseTab);
        _fileWatcher.ReportParsed += OnReportParsed;
        _fileWatcher.WatchFolder();
    }

    private void OnReportParsed(MultiplayerCarnageReport report)
    {
        Dispatcher.Invoke(() =>
        {
            var duplicateGame = Games.FirstOrDefault(g => g.GameUniqueId == report.GameUniqueId.Value);
            if (duplicateGame is not null)
            {
                Games.Remove(duplicateGame);
            }

            var game = new Game
            {
                GameUniqueId = report.GameUniqueId.Value,
                GameTypeName = report.GameTypeName.Value,
                PlayerScores = report.Players.Select(p => new PlayerScore
                {
                    TeamId = p.mTeamId,
                    GamerTag = p.mGamertagText,
                    Kills = p.mKills,
                    Assists = p.mAssists,
                    Deaths = p.mDeaths,
                    Score = p.Score
                }).ToList()
            };

            Games.Add(game);
            SelectedGameIndex = Games.Count - 1; // Select the last tab
        });
    }

    private void CloseTab(Game? game)
    {
        if (game != null)
        {
            int index = Games.IndexOf(game);
            Games.Remove(game);
            // Adjust SelectedGameIndex if needed
            if (SelectedGameIndex >= Games.Count)
                SelectedGameIndex = Games.Count - 1;
            else if (SelectedGameIndex > index)
                SelectedGameIndex--;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}