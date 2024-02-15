using System.Configuration;
using System.Data;
using System.Windows;
using MyApp.MVVM.ViewModel;
using MyApp.Store;

namespace MyApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            this.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;

            string rootDir = System.IO.Directory.GetCurrentDirectory();
            string dataDir = rootDir + "\\data\\";
            string dataFile = dataDir + "data.json";
            string tmpDir = dataDir + "tmp\\";

            DataStore dataStore = new DataStore(dataDir, dataFile, tmpDir);

            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowVM(
                    dataStore,
                    (obj) => { MainWindow.Close(); },
                    (obj) => { MainWindow.WindowState = WindowState.Minimized; }
                )
            };

            MainWindow.Show();

            base.OnStartup(e);
        }
    }

}
