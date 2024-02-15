using System.Windows.Input;
using MyApp.MVVM.View;
using MyApp.MVVM.ViewModel.Commands;
using MyApp.Store;

namespace MyApp.MVVM.ViewModel
{
    public class NavbarVM : VMBase
    {
        public NavbarVM(NavigationStore navigationStore)
        {
            cmdNavEmptyView = new NavigateCommand<VMBase>(navigationStore, navigationStore.EmptyViewVM);
        }

        public ICommand cmdNavEmptyView { get; }
    }
}
