using System.Collections.ObjectModel;
using Microsoft.Reporting.WinForms;
using WeighBridgeApplication.RustiviaService.DbContext;
using WeighBridgeApplication.Util;

namespace WeighBridgeApplication.ViewModel
{
    public class ReprintInvoiceViewModel : ViewModelBase
    {
        private ObservableCollection<WeighBridgeInfo> _weighBridgeInfos;
        private readonly PrintTicket _slipPrintTicket = new PrintTicket();
        private WeighBridgeInfo _selectedIncompleteWeighBridgeInfo;

        public ReprintInvoiceViewModel()
        {
            SomeShitGoingOnCommand = new DelegateCommand(SomeShitGoingOn);
            GetWeighBridgeInfo();
        }

        private void SomeShitGoingOn()
        {
//            ReportWeighBridge report = new ReportWeighBridge();
            PrintReceipt(SelectedIncompleteWeighBridgeInfo);

//            _slipPrintTicket.PrintTicketSlip(SelectedIncompleteWeighBridgeInfo);
        }
        public WeighBridgeInfo SelectedIncompleteWeighBridgeInfo
        {
            get { return _selectedIncompleteWeighBridgeInfo; }
            set
            {
                if (_selectedIncompleteWeighBridgeInfo != value)
                {
                    _selectedIncompleteWeighBridgeInfo = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<WeighBridgeInfo> WeighBridgeInfos
        {
            get { return _weighBridgeInfos; }
            set
            {
                if (_weighBridgeInfos != value)
                {
                    _weighBridgeInfos = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand SomeShitGoingOnCommand { get; set; }

        private void GetWeighBridgeInfo()
        {
            WeighBridgeInfoDto[] allWeight = ContextClient.GetWeighBridgeInfoAsync().Result;
            WeighBridgeInfos = new ObservableCollection<WeighBridgeInfo>();

            foreach (WeighBridgeInfoDto weighBridgeInfo in allWeight)
            {
                if (weighBridgeInfo.Status != "FirstWeight")
                {
                    var weighBridge = new WeighBridgeInfo
                    {
                        Id = weighBridgeInfo.Id,
                        FirstMass = weighBridgeInfo.FirstMass,
                        SecondMass = weighBridgeInfo.SecondMass,
                        NettMass = weighBridgeInfo.NettMass,
                        Date = weighBridgeInfo.Date,
                        Product = weighBridgeInfo.Product,
                        Comments = weighBridgeInfo.Comments,
                        Driver = GetDriver(weighBridgeInfo.Driver),
                        Truck = GetTruck(weighBridgeInfo.TruckDto)
                    };
                    WeighBridgeInfos.Add(weighBridge);
                }
            }
        }
    }
}