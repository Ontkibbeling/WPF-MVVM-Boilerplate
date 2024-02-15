using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyApp.MVVM.ViewModel.Commands;

namespace MyApp.MVVM.ViewModel
{
    public class WindowBarVM : VMBase
    {
        public WindowBarVM(Action<object> closeApp, Action<object> minimize, string appVersion)
        {
            AppVersion = appVersion;
            btnClose = new RelayCommand(closeApp);
            btnMinimize = new RelayCommand(minimize);
        }

        public string AppVersion { get; }
        public ICommand btnClose { get; }
        public ICommand btnMinimize { get; }
    }
}
