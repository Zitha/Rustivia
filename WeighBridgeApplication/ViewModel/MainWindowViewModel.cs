// Decompiled with JetBrains decompiler
// Type: WeighBridgeApplication.ViewModel.MainWindowViewModel
// Assembly: WeighBridgeApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0008ECA4-ECDC-4057-A6F3-F7FD4CE2F23F
// Assembly location: C:\Users\Ndavhe\Desktop\Rustivia Weight Bridge\WeighBridgeApplication.exe

using System;
using System.Windows;
using System.Windows.Controls;
using WeighBridgeApplication.Util;
using WeighBridgeApplication.View;

namespace WeighBridgeApplication.ViewModel
{
    public class MainWindowViewModel : PropertyChangedNotify
    {
        private readonly SerialPortReader _serialPortReader;
        private bool _isBusy;
        private UserControl _weighBridgeContentView;
        private bool _isButtonVisible;

        public MainWindowViewModel(string userName)
        {
            this.WeighModeCommand = new DelegateCommand(new Action(this.OpenWeighMode));
            this.InvoiceReprintCommand = new DelegateCommand(new Action(this.OpenInvoiceReprint));
            this.CloseWindowCommand = new DelegateCommand(new Action(this.CloseWindow));
            this.ExitCommand = new DelegateCommand(new Action(this.Exit));
            this._serialPortReader = new SerialPortReader();
            this._isButtonVisible = Program.IsServiceDown;
            this.LoadMain();
        }

        public DelegateCommand WeighModeCommand { get; set; }

        public DelegateCommand InvoiceReprintCommand { get; set; }

        public DelegateCommand CloseWindowCommand { get; set; }

        public DelegateCommand ExitCommand { get; set; }

        public UserControl WeighBridgeContentView
        {
            get
            {
                return this._weighBridgeContentView;
            }
            set
            {
                if (this._weighBridgeContentView == value)
                    return;
                this._weighBridgeContentView = value;
                this.OnPropertyChanged(nameof(WeighBridgeContentView));
            }
        }

        public bool IsBusy
        {
            get
            {
                return this._isBusy;
            }
            set
            {
                this._isBusy = value;
                this.OnPropertyChanged(nameof(IsBusy));
            }
        }

        public bool IsButtonVisible
        {
            get
            {
                return this._isButtonVisible;
            }
            set
            {
                this._isButtonVisible = value;
                this.OnPropertyChanged(nameof(IsButtonVisible));
            }
        }

        public void LoadMain()
        {
            this.WeighBridgeContentView = (UserControl)new MainPageAdvert();
        }

        private void Exit()
        {
            if (MessageBox.Show("Close Application?", "Exit Application", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            Program.CloseApplication();
        }

        private void CloseWindow()
        {
            this.LoadMain();
        }

        private void OpenInvoiceReprint()
        {
            try
            {
                this.IsBusy = true;
                this._serialPortReader.Close();
                ReprintInvoiceViewModel invoiceViewModel1 = new ReprintInvoiceViewModel();
                ReprintInvoiceView reprintInvoiceView = new ReprintInvoiceView();
                ReprintInvoiceViewModel invoiceViewModel2 = invoiceViewModel1;
                reprintInvoiceView.DataContext = (object)invoiceViewModel2;
                this.WeighBridgeContentView = (UserControl)reprintInvoiceView;
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                int num = (int)MessageBox.Show(string.Format("Error occured with Message {0}", (object)ex.Message));
            }
            this.IsBusy = false;
        }

        private void OpenWeighMode()
        {
            try
            {
                this.IsBusy = true;
                WeighModeViewModel weighModeViewModel1 = new WeighModeViewModel(this._serialPortReader);
                WeighModeView weighModeView = new WeighModeView();
                WeighModeViewModel weighModeViewModel2 = weighModeViewModel1;
                weighModeView.DataContext = (object)weighModeViewModel2;
                this.WeighBridgeContentView = (UserControl)weighModeView;
                this.IsBusy = false;
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(string.Format("Error occured with Message {0}", (object)ex.Message));
            }
        }
    }
}
