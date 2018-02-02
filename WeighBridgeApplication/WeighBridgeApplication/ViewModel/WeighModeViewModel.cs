using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Windows.Controls;
using WeighBridgeApplication.RustiviaService.DbContext;
using WeighBridgeApplication.Util;
using WeighBridgeApplication.View;

namespace WeighBridgeApplication.ViewModel
{
    public class WeighModeViewModel : ViewModelBase
    {
        private readonly PrintTicket _sliPrintTicket = new PrintTicket();
        private bool _canProccess;
        private string _caption;
        private decimal _firstMass;
        private ObservableCollection<WeighBridgeInfo> _inCompleteWeighBridgeInfo;
        private bool _isConnected;
        private bool _isNewWeightSelected;
        private decimal _nettMass;
        private Options _options = Options.Collection;
        private string _processStatus;
        private string _scaleNettMass;
        private decimal _secondMass;
        private WeighBridgeInfo _selectedIncompleteWeighBridgeInfo;
        private SerialPort _sp;
        private string _strCardNo = "";
        private SupplyViewModel _supplyViewModel;
        private decimal _weighBridgeScale;
        private int _weighCounter;
        private UserControl _weighOptionView;

        public WeighModeViewModel()
        {
            Initialize();
        }

        public SupplyViewModel SupplyViewModel
        {
            get { return _supplyViewModel; }
            set
            {
                if (_supplyViewModel != value)
                {
                    _supplyViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsNewWeightSelected
        {
            get { return _isNewWeightSelected; }
            set
            {
                if (_isNewWeightSelected != value)
                {
                    _isNewWeightSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public Options Options
        {
            get { return _options; }
            set
            {
                _options = value;
                OnPropertyChanged();

                if (Options.ToString() == "Delivery")
                {
                    ResertWeightBridge();
                    WeighOptionView = new DeliveryView();
                }
                if (Options.ToString() == "Collection")
                {
                    ResertWeightBridge();
                    WeighOptionView = new SupplyView {DataContext = SupplyViewModel};
                }
            }
        }

        public string ScaleNettMass
        {
            get { return _scaleNettMass; }
            set
            {
                if (_scaleNettMass != value)
                {
                    _scaleNettMass = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Caption
        {
            get { return _caption; }
            set
            {
                if (_caption != value)
                {
                    _caption = value;
                    OnPropertyChanged();
                }
            }
        }

        public UserControl WeighOptionView
        {
            get { return _weighOptionView; }
            set
            {
                if (_weighOptionView != value)
                {
                    _weighOptionView = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal SecondMass
        {
            get { return _secondMass; }
            set
            {
                if (_secondMass != value)
                {
                    _secondMass = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal NettMass
        {
            get { return _nettMass; }
            set
            {
                if (_nettMass != value)
                {
                    _nettMass = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal FirstMass
        {
            get { return _firstMass; }
            set
            {
                if (_firstMass != value)
                {
                    _firstMass = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal WeighBridgeScale
        {
            get { return _weighBridgeScale; }
            set
            {
                if (_weighBridgeScale != value)
                {
                    _weighBridgeScale = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<WeighBridgeInfo> InCompleteWeighBridgeInfo
        {
            get { return _inCompleteWeighBridgeInfo; }
            set
            {
                if (_inCompleteWeighBridgeInfo != value)
                {
                    _inCompleteWeighBridgeInfo = value;
                    OnPropertyChanged();
                }
            }
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

        public DelegateCommand ProcessCommand { get; set; }
        public DelegateCommand SaveDetailsCommand { get; set; }
        public DelegateCommand ResertDetailsCommand { get; set; }
        public DelegateCommand SomeShitGoingOnCommand { get; set; }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanProccess
        {
            get { return _canProccess; }
            set
            {
                if (_canProccess != value)
                {
                    _canProccess = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ResertWeightBridge()
        {
            FirstMass = 0;
            SecondMass = 0;
            Caption = "First Weight";
        }

        private void GetSerialPort()
        {
            try
            {
                _sp = new SerialPort("COM6", 19200, Parity.None, 8, StopBits.One);
                _sp.DataReceived += _sp_DataReceived;
                _sp.Open();
            }
            catch (Exception)
            {
                IsConnected = false;
            }
        }

        private void _sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (!_sp.IsOpen)
                    return;
                string str1 = _sp.ReadExisting();

                if (!string.IsNullOrEmpty(str1))
                {
                    _strCardNo = str1;
                    if (_strCardNo.Length < 15)
                        return;
                    string str2 = _strCardNo.Substring(2, 6);

                    int num1 = Convert.ToInt32(str2);
                    WeighBridgeScale = num1;
                }
                else
                {
                    _sp.BreakState = true;
                    _sp.BreakState = false;
                }
            }
            catch (Exception ex)
            {
                IsConnected = false;
            }
        }


        private void ProcessWeight()
        {
            if (_weighCounter == 0)
            {
                FirstMass = WeighBridgeScale;
                Caption = "Second Weight";
                _processStatus = "FirstWeight";
            }
            if (_weighCounter == 1)
            {
                SecondMass = WeighBridgeScale;
                _processStatus = "UnProcessed";
                CanProccess = false;
                GetNettMass();
            }
            _weighCounter++;
        }

        private void GetNettMass()
        {
            NettMass = FirstMass - SecondMass;
            ScaleNettMass = string.Format("{0} KG", NettMass);
        }

        private void Initialize()
        {
            IsNewWeightSelected = true;
            IsConnected = true;
            Caption = "First weight";
            CanProccess = true;
            _weighCounter = 0;
            SecondMass = 0;
            FirstMass = 0;
            NettMass = 0;
            SupplyViewModel = new SupplyViewModel();
            GetSerialPort();
            ProcessCommand = new DelegateCommand(ProcessWeight);
            SaveDetailsCommand = new DelegateCommand(SaveToDataBase);
            SomeShitGoingOnCommand = new DelegateCommand(SomeShitGoingOn);
            ResertDetailsCommand = new DelegateCommand(Initialize);
            GetIncompleteWeights();
            WeighOptionView = new SupplyView {DataContext = SupplyViewModel};
        }

        private void SomeShitGoingOn()
        {
            SupplyViewModel = new SupplyViewModel(SelectedIncompleteWeighBridgeInfo);
            WeighOptionView = new SupplyView {DataContext = SupplyViewModel};
            FirstMass = SelectedIncompleteWeighBridgeInfo.FirstMass;
            IsNewWeightSelected = true;
            _weighCounter = 1;
            Caption = "Second Weight";
        }

        private void GetIncompleteWeights()
        {
            InCompleteWeighBridgeInfo = new ObservableCollection<WeighBridgeInfo>();
            WeighBridgeInfoDto[] allWeight = ContextClient.GetWeighBridgeInfoAsync().Result;

            foreach (WeighBridgeInfoDto weighBridgeInfoDto in allWeight)
            {
                if (weighBridgeInfoDto.Status == "FirstWeight")
                {
                    InCompleteWeighBridgeInfo.Add(new WeighBridgeInfo
                    {
                        Id = weighBridgeInfoDto.Id,
                        FirstMass = weighBridgeInfoDto.FirstMass,
                        SecondMass = weighBridgeInfoDto.SecondMass,
                        NettMass = weighBridgeInfoDto.NettMass,
                        Date = weighBridgeInfoDto.Date,
                        Product = weighBridgeInfoDto.Product,
                        Comments = weighBridgeInfoDto.Comments,
                        Driver = GetDriver(weighBridgeInfoDto.Driver),
                        Truck = GetTruck(weighBridgeInfoDto.TruckDto)
                    });
                }
            }
        }

        private void SaveToDataBase()
        {
            if (SelectedIncompleteWeighBridgeInfo == null)
            {
                var driverDto = new DriverDto
                {
                    Id = SupplyViewModel.SelectedDriver.Id,
                    Firstname = SupplyViewModel.SelectedDriver.Firstname,
                    Surname = SupplyViewModel.SelectedDriver.Surname,
                    IdNumber = SupplyViewModel.SelectedDriver.IdNumber
                };
                var truckDto = new TruckDto
                {
                    Id = SupplyViewModel.SelectedTruck.Id,
                    TruckRegNumber = SupplyViewModel.SelectedTruck.TruckRegNumber
                };

                var weighBridge = new WeighBridgeInfoDto
                {
                    SecondMass = SecondMass,
                    FirstMass = FirstMass,
                    Date = DateTime.Now.Date,
                    Driver = driverDto,
                    NettMass = NettMass,
                    TruckDto = truckDto,
                    Product = SupplyViewModel.ProductType,
                    Comments = SupplyViewModel.Comments,
                    Status = _processStatus
                };

                ContextClient.AddWeighBridgeInfo(weighBridge);
                GetIncompleteWeights();
                if (_weighCounter == 2)
                {
                    WeighBridgeInfo incompleteWeight =
                        InCompleteWeighBridgeInfo.SingleOrDefault(
                            d => d.Driver.Id == driverDto.Id && d.Date == DateTime.Now.Date);
                    if (incompleteWeight != null)
                    {
                        PrintReceipt(new WeighBridgeInfo
                        {
                            SecondMass = SecondMass,
                            FirstMass = FirstMass,
                            Date = DateTime.Now,
                            Driver = new Driver
                            {
                                Id = SupplyViewModel.SelectedDriver.Id,
                                Firstname = SupplyViewModel.SelectedDriver.Firstname,
                                Surname = SupplyViewModel.SelectedDriver.Surname,
                                IdNumber = SupplyViewModel.SelectedDriver.IdNumber
                            },
                            NettMass = NettMass,
                            Truck = new Truck
                            {
                                Id = SupplyViewModel.SelectedTruck.Id,
                                TruckRegNumber = SupplyViewModel.SelectedTruck.TruckRegNumber
                            },
                            Product = SupplyViewModel.ProductType,
                            Comments = SupplyViewModel.Comments,
                            Status = _processStatus,
                            Id = incompleteWeight.Id
                        });
                    }
                    Initialize();
                }
            }
            else
            {
                var weighBridge = new WeighBridgeInfoDto
                {
                    Id = SelectedIncompleteWeighBridgeInfo.Id,
                    SecondMass = SecondMass,
                    FirstMass = SelectedIncompleteWeighBridgeInfo.FirstMass,
                    NettMass = NettMass,
                    Status = _processStatus
                };
                ContextClient.UpdateWeighBridgeInfo(weighBridge);
                PrintReceipt(SelectedIncompleteWeighBridgeInfo);
                Initialize();
            }
        }
    }
}