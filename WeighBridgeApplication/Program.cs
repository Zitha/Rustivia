using System;
using System.Diagnostics;
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
        public static bool IsServiceDown;


        [STAThread]
        private static void Main()
        {
            try
            {
                //Program.CreateEventLog();
                _application = new Application();
                _controller = new ApplicationController(_application);
                _logon = new LogonView();
                LogonViewModel logonViewModel = new LogonViewModel();
                _logon.DataContext = (object)logonViewModel;
                _logon.ShowDialog();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("Application failed: " + (object)ex);
            }
        }

        private static void CreateEventLog()
        {
            if (EventLog.SourceExists("WeighBridgeSource"))
                return;
            EventLog.CreateEventSource("WeighBridgeSource", "WeighBridgeLog");
        }

        public static void ValidLogon(string userName)
        {
            _controller.Run(userName);
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