using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyApp.MVVM.ViewModel.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute; 

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) 
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // Ik weet zelf niet hoe dit werkt...
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);

        protected void OnCanExecuteChanged() //Deze method zelf gecopy-paste. Not sure of het goed is.
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
