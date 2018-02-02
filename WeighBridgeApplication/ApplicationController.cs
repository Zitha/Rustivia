using System;
using System.Windows;
using WeighBridgeApplication.View;
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
                MessageBox.Show("Error occured in the application " + exception.Message);
            }
        }

        internal void Run(string userName)
        {
            var mainWindow = new MainWindow { Title = "Weight Bridge Application" };
            _mainViewModel = new MainWindowViewModel(userName);
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.DataContext = _mainViewModel;
            Program.CloseLogon();
            _application.Run(mainWindow);
        }
    }
}