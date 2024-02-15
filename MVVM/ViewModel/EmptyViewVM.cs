using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MyApp.MVVM.ViewModel.Commands;
using MyApp.Store;
using System.Linq;
using System.Diagnostics;
using System.Windows.Navigation;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media.Animation;

namespace MyApp.MVVM.ViewModel
{
    public class EmptyViewVM : VMNotifyErrorInfoBase
    {
        private readonly DataStore _dataStore;

        public EmptyViewVM(DataStore dataStore)
        {
            _dataStore = dataStore;
            TmpDir = _dataStore.TmpDir;
        }


        public string TmpDir { get; }
    }
}
