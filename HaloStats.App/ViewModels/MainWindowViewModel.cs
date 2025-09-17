using HaloStats.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
