using System;
using System.Windows;
using System.Windows.Controls;
using WeighBridgeApplication.View;

namespace WeighBridgeApplication.ViewModel
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        private UserControl _weighBridgeContentView;

        public MainWindowViewModel()
        {
            WeighModeCommand = new DelegateCommand(OpenWeighMode);
            InvoiceReprintCommand = new DelegateCommand(OpenInvoiceReprint);
            CloseWindowCommand = new DelegateCommand(CloseWindow);
            ExitCommand = new DelegateCommand(Exit);
            LoadMain();
        }

        public DelegateCommand WeighModeCommand { get; set; }
        public DelegateCommand InvoiceReprintCommand { get; set; }
        public DelegateCommand CloseWindowCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }

        public UserControl WeighBridgeContentView
        {
            get { return _weighBridgeContentView; }
            set
            {
                if (_weighBridgeContentView != value)
                {
                    _weighBridgeContentView = value;
                    OnPropertyChanged();
                }
            }
        }

        public void Dispose()
        {
            ContextClient.Close();
        }

        public void LoadMain()
        {
            var mainAdvertView = new MainPageAdvert();
            WeighBridgeContentView = mainAdvertView;
        }

        private void Exit()
        {
            if (MessageBox.Show("Close Application?", "Close", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                MessageBoxResult.Yes)
            {
                Program.CloseApplication();
            }
        }


        private void CloseWindow()
        {
            LoadMain();
        }

        private void OpenInvoiceReprint()
        {
            var weighModeViewModel = new ReprintInvoiceViewModel();
            var reprintInvoiceView = new ReprintInvoiceView {DataContext = weighModeViewModel};
            WeighBridgeContentView = reprintInvoiceView;
        }

        private void OpenWeighMode()
        {
            var weighModeViewModel = new WeighModeViewModel();
            var weighModeView = new WeighModeView {DataContext = weighModeViewModel};
            WeighBridgeContentView = weighModeView;
        }
    }
}