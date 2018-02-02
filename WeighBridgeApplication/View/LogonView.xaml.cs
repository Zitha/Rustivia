using System.Windows;
using System.Windows.Controls;
using WeighBridgeApplication.Util;

namespace DocumentManagement.View
{
    /// <summary>
    ///     Interaction logic for LogonView.xaml
    /// </summary>
    public partial class LogonView : Window
    {
        public LogonView()
        {
            InitializeComponent();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var pBox = sender as PasswordBox;

            //Set this "EncryptedPassword" dependency property to the "SecurePassword"
            //of the PasswordBox.
            PasswordBoxMvvmAttachedProperties.SetEncryptedPassword(pBox, pBox.SecurePassword);
        }
    }
}