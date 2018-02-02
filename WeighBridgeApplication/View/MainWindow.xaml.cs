using System;
using System.Diagnostics;
using System.Windows;
using DocumentManagement.View;
using WeighBridgeApplication.Util;
using WeighBridgeApplication.ViewModel;

namespace WeighBridgeApplication.View
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //            HwndSource windowSpecificOSMessageListener = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            //            windowSpecificOSMessageListener.AddHook(CallBackMethod);
            //            string strlogOffTime = ConfigurationManager.AppSettings["LogOffTime"];
            //            AutoLogOffHelper.LogOffTime = Convert.ToInt32(strlogOffTime);
            //            AutoLogOffHelper.MakeAutoLogOffEvent += AutoLogOffHelper_MakeAutoLogOffEvent;
            //            AutoLogOffHelper.StartAutoLogoffOption();
            if (Program.IsServiceDown)
            {
                offLineBtn.Visibility = Visibility.Visible;
            }
            else
            {
                offLineBtn.Visibility = Visibility.Hidden;
            }
         
        }

        private IntPtr CallBackMethod(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //  Listening OS message to test whether it is a user activity
            if ((msg >= 0x0200 && msg <= 0x020A) || (msg <= 0x0106 && msg >= 0x00A0) || msg == 0x0021)
            {
                AutoLogOffHelper.ResetLogoffTimer();
            }
            else
            {
                // For debugging purpose
                // If this auto logoff does not work for some user activity, you can detect the integer code of that activity  using the following line.
                //Then All you need to do is adding this integer code to the above if condition.
                Debug.WriteLine(msg.ToString());
            }
            return IntPtr.Zero;
        }

        private void AutoLogOffHelper_MakeAutoLogOffEvent()
        {
            Logoff();
        }

        private void Logoff()
        {
            var viewModel = new LogonViewModel();
            var mainWindow = new LogonView { DataContext = viewModel };
            mainWindow.ShowDialog();
        }
    }
}