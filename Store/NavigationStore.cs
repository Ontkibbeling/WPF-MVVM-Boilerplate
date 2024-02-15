using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.MVVM.ViewModel;

namespace MyApp.Store
{
    public class NavigationStore
    {
        public NavigationStore(EmptyViewVM emptyViewVM)
        {
            EmptyViewVM = emptyViewVM;
            CurrentViewModel = emptyViewVM; //default at startup
        }

        private VMBase _CurrentViewModel;
        public VMBase CurrentViewModel
        {
            get => _CurrentViewModel;
            set
            {
                _CurrentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        public EmptyViewVM EmptyViewVM { get; }
        
        public event Action CurrentViewModelChanged;

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
