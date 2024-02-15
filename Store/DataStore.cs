using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using MyApp.Utilities;

namespace MyApp.Store
{
    public class DataStore: INotifyPropertyChanged, INotifyCollectionChanged
    {
        private string _dataDir;
        private string _dataFile;
        private string _gemeenteFile;
        private string? _doelmap;
        private string? _datasmap;

        public DataStore(string dataDir, string dataFile, string tmpDir) 
        {
            _dataDir = dataDir;
            _dataFile = dataFile;
            TmpDir = tmpDir;

            JHandler = new JsonHandler(_dataFile);

            initialiseDataFiles();
            initialiseDefaults();
        }

        public string TmpDir { get; }
        public string? Doelmap
        {
            get => _doelmap;
            set
            {
                _doelmap = value;
                JHandler.InsertDefault(nameof(Doelmap), value);
            }
        }
        public string? Datasmap
        {
            get => _datasmap;
            set
            {
                _datasmap = value;
                JHandler.InsertDefault(nameof(Datasmap), value);
            }
        }
        public JsonHandler JHandler { get; private set; }

        #region methods
        private void initialiseDefaults()
        {
            _doelmap = JHandler.GetDefault(nameof(Doelmap));
            _datasmap = JHandler.GetDefault(nameof(Datasmap));
        }
        private void initialiseDataFiles()
        {
            createDataFileIfAbsent();
        }
        private void createDataFileIfAbsent()
        {
            string dataFolder = _dataDir;
            string file = _dataFile;
            string tmpFolder = TmpDir;

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            if (!Directory.Exists(tmpFolder))
            {
                Directory.CreateDirectory(tmpFolder);
            }

            if (!File.Exists(file))
            {
                File.Create(file).Close();
            }

            string allContent = File.ReadAllText(file);

            if (string.IsNullOrEmpty(allContent))
            {
                JHandler.GenerateEmptyJson();
            }
        }

        #endregion methods

        #region INotify members

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        #endregion INotify members
    }
}
