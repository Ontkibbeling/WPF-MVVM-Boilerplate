using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MyApp.MVVM.ViewModel.Commands;
using MyApp.Store;

namespace MyApp.MVVM.ViewModel
{
    public class MainWindowVM: VMBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly string _appVersion = "Version 0.0.0";

        public MainWindowVM(DataStore dataStore, Action<object> closeWindow, Action<object> minimize) 
        {
            WindowBarVM = new WindowBarVM(closeWindow, minimize, _appVersion);
            EmptyViewVM emptyViewVM = new EmptyViewVM(dataStore);

            _navigationStore = new NavigationStore(emptyViewVM);
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            NavbarVM = new NavbarVM(_navigationStore);
        }

        
        public VMBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public NavbarVM NavbarVM { get; } 
        public WindowBarVM WindowBarVM { get; }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }
}
