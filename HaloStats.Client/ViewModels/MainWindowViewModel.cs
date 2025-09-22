using System.Collections.ObjectModel;

namespace HaloStats.Client.ViewModels
{
    internal class MainWindowViewModel
    {
        public ObservableCollection<Game> Games { get; } = new();

        public MainWindowViewModel()
        {

        }
    }
}
