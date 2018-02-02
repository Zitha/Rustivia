using System;
using System.Windows;
using WeighBridgeApplication.ViewModel;

namespace WeighBridgeApplication
{
    public class ApplicationController
    {
        private readonly Application _application;
        private MainWindowViewModel _mainViewModel;

        public ApplicationController(Application application)
        {
            try
            {
                _application = application;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to load Application " + exception.Message);
            }
        }

        internal void Run()
        {
            var mainWindow = new MainWindow { Title = "Weight Bridge Application" };
            _mainViewModel = new MainWindowViewModel();
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.DataContext = _mainViewModel;
            Program.CloseLogon();
            _application.Run(mainWindow);
        }

    }
}