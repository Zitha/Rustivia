using System;
using System.Windows;
using DocumentManagement.View;
using WeighBridgeApplication.RustiviaService.DbContext;
using WeighBridgeApplication.ViewModel;

namespace WeighBridgeApplication
{
    public class Program
    {
        private static LogonView _logon;
        private static Application _application;
        private static ApplicationController _controller;
        [STAThread]
        private static void Main()
        {
            try
            {
                var client = new DataContextClient();
                _application = new Application();
                _controller = new ApplicationController(_application);
                _logon = new LogonView();
                var viewModel = new LogonViewModel(client);
                _logon.DataContext = viewModel;
                _logon.ShowDialog();
                //                ValidLogon("");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Application failed: " + exception);
            }
        }

        public static void ValidLogon(string userName)
        {
            _controller.Run();
        }
        public static void CloseLogon()
        {
            _logon.Close();
        }

        public static void CloseApplication()
        {
            _application.Shutdown();
        }
    }
}