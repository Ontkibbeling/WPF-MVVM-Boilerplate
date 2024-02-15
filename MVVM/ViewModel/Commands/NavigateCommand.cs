using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.MVVM.ViewModel;
using MyApp.Store;

namespace MyApp.MVVM.ViewModel.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : VMBase
    {
        public NavigateCommand(NavigationStore navigationStore, VMBase newViewModel)
        {
            _NavigationStore = navigationStore;
            _NewViewModel = newViewModel;
        }

        private readonly NavigationStore _NavigationStore;
        private readonly VMBase _NewViewModel;

        public override void Execute(object? parameter)
        {
            _NavigationStore.CurrentViewModel = _NewViewModel;
        }
    }
}
