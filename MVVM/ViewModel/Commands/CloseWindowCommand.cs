using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyApp.MVVM.ViewModel.Commands
{
    class CloseWindowCommand : CommandBase
    {
        public CloseWindowCommand(Action closeAction)
        {
            CloseAction = closeAction;
        }

        public Action CloseAction { get; }
        public override void Execute(object parameter)
        {
            CloseAction();
        }
    }
}
