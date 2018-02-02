// Decompiled with JetBrains decompiler
// Type: WeighBridgeApplication.ViewModel.WeighModeViewModel
// Assembly: WeighBridgeApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0008ECA4-ECDC-4057-A6F3-F7FD4CE2F23F
// Assembly location: C:\Users\Ndavhe\Desktop\Rustivia Weight Bridge\WeighBridgeApplication.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WeighBridgeApplication.Model;
using WeighBridgeApplication.OfflineMode;
using WeighBridgeApplication.Util;
using WeighBridgeApplication.View;

namespace WeighBridgeApplication.ViewModel
{
    public class WeighModeViewModel : ViewModelBase, IDisposable
    {
        private Options _options = Options.Purchase;
        private string _caption;
        private DeliveryViewModel _deliveryViewModel;
        private long _firstMass;
        private ObservableCollection<Container> _inCompleteContainers;
        private ObservableCollection<Purchase> _inCompletePurchases;
        private ObservableCollection<Sale> _inCompleteSales;
        private bool _isBusy;
        private bool _isConnected;
        private bool _isFirstWeighSaved;
        private bool _isModelValid;
        private bool _isNewWeightSelected;
        private long _nettMass;
        private string _processStatus;
        private Purchase _purchase;
        private string _scaleNettMass;
        private long _secondMass;
        private Container _selectedIncompleteContainer;
        private Purchase _selectedIncompletePurchase;
        private Sale _selectedIncompleteSale;
        private SupplyViewModel _supplyViewModel;
        private long _weighBridgeScale;
        private int _weighCounter;
        private UserControl _weighOptionView;

        public WeighModeViewModel(SerialPortReader serialPortReader)
        {
            this.IsConnected = serialPortReader.Open();
            serialPortReader.ScaleValueEvent += new EventHandler<SerialportEventArgs>(this.serialPortReader_ScaleValueEvent);
            this.Initialize();
        }

        public SupplyViewModel SupplyViewModel
        {
            get
            {
                return this._supplyViewModel;
            }
            set
            {
                if (this._supplyViewModel == value)
                    return;
                this._supplyViewModel = value;
                this.OnPropertyChanged(nameof(SupplyViewModel));
            }
        }

        public DeliveryViewModel DeliveryViewModel
        {
            get
            {
                return this._deliveryViewModel;
            }
            set
            {
                if (this._deliveryViewModel == value)
                    return;
                this._deliveryViewModel = value;
                this.OnPropertyChanged(nameof(DeliveryViewModel));
            }
        }

        public bool IsNewWeightSelected
        {
            get
            {
                return this._isNewWeightSelected;
            }
            set
            {
                if (this._isNewWeightSelected == value)
                    return;
                this._isNewWeightSelected = value;
                this.OnPropertyChanged(nameof(IsNewWeightSelected));
            }
        }

        public Options Options
        {
            get
            {
                return this._options;
            }
            set
            {
                this._options = value;
                this.OnPropertyChanged(nameof(Options));
                if (this.Options.ToString() == "Sale" && (this.SelectedIncompleteContainer == null && this.SelectedIncompleteSale == null))
                {
                    this.IsBusy = true;
                    this.ResertWeightBridge();
                    this.DeliveryViewModel = new DeliveryViewModel((Container)null, (Sale)null);
                    this.DeliveryViewModel.IsValidEvent += new EventHandler(this.IsValidModelValidEventCheck);
                    DeliveryView deliveryView = new DeliveryView();
                    DeliveryViewModel deliveryViewModel = this.DeliveryViewModel;
                    deliveryView.DataContext = (object)deliveryViewModel;
                    this.WeighOptionView = (UserControl)deliveryView;
                    this.IsBusy = false;
                }
                if (!(this.Options.ToString() == "Purchase") || this.SelectedIncompletePurchase != null)
                    return;
                this.ResertWeightBridge();
                this.SupplyViewModel = new SupplyViewModel((Purchase)null);
                this.SupplyViewModel.IsValidEvent += new EventHandler(this.IsValidModelValidEventCheck);
                SupplyView supplyView = new SupplyView();
                SupplyViewModel supplyViewModel = this.SupplyViewModel;
                supplyView.DataContext = (object)supplyViewModel;
                this.WeighOptionView = (UserControl)supplyView;
            }
        }

        public bool IsModelValid
        {
            get
            {
                return this._isModelValid;
            }
            set
            {
                if (this._isModelValid == value)
                    return;
                this._isModelValid = value;
                this.OnPropertyChanged(nameof(IsModelValid));
            }
        }

        public string ScaleNettMass
        {
            get
            {
                return this._scaleNettMass;
            }
            set
            {
                if (!(this._scaleNettMass != value))
                    return;
                this._scaleNettMass = value;
                this.OnPropertyChanged(nameof(ScaleNettMass));
            }
        }

        public string Caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                if (!(this._caption != value))
                    return;
                this._caption = value;
                this.OnPropertyChanged(nameof(Caption));
            }
        }

        public UserControl WeighOptionView
        {
            get
            {
                return this._weighOptionView;
            }
            set
            {
                if (this._weighOptionView == value)
                    return;
                this._weighOptionView = value;
                this.OnPropertyChanged(nameof(WeighOptionView));
            }
        }

        public long SecondMass
        {
            get
            {
                return this._secondMass;
            }
            set
            {
                if (this._secondMass == value)
                    return;
                this._secondMass = value;
                this.OnPropertyChanged(nameof(SecondMass));
            }
        }

        public long NettMass
        {
            get
            {
                return this._nettMass;
            }
            set
            {
                if (this._nettMass == value)
                    return;
                this._nettMass = value;
                this.OnPropertyChanged(nameof(NettMass));
            }
        }

        public long FirstMass
        {
            get
            {
                return this._firstMass;
            }
            set
            {
                if (this._firstMass == value)
                    return;
                this._firstMass = value;
                this.OnPropertyChanged(nameof(FirstMass));
            }
        }

        public long WeighBridgeScale
        {
            get
            {
                return this._weighBridgeScale;
            }
            set
            {
                if (this._weighBridgeScale == value)
                    return;
                this._weighBridgeScale = value;
                this.OnPropertyChanged(nameof(WeighBridgeScale));
            }
        }

        public ObservableCollection<Purchase> InCompletePurchases
        {
            get
            {
                return this._inCompletePurchases;
            }
            set
            {
                if (this._inCompletePurchases == value)
                    return;
                this._inCompletePurchases = value;
                this.OnPropertyChanged(nameof(InCompletePurchases));
            }
        }

        public ObservableCollection<Sale> InCompleteSales
        {
            get
            {
                return this._inCompleteSales;
            }
            set
            {
                if (this._inCompleteSales == value)
                    return;
                this._inCompleteSales = value;
                this.OnPropertyChanged(nameof(InCompleteSales));
            }
        }

        public Purchase SelectedIncompletePurchase
        {
            get
            {
                return this._selectedIncompletePurchase;
            }
            set
            {
                if (this._selectedIncompletePurchase == value)
                    return;
                this._selectedIncompletePurchase = value;
                this.OnPropertyChanged(nameof(SelectedIncompletePurchase));
            }
        }

        public Container SelectedIncompleteContainer
        {
            get
            {
                return this._selectedIncompleteContainer;
            }
            set
            {
                if (this._selectedIncompleteContainer == value)
                    return;
                this._selectedIncompleteContainer = value;
                this.OnPropertyChanged(nameof(SelectedIncompleteContainer));
            }
        }

        public Sale SelectedIncompleteSale
        {
            get
            {
                return this._selectedIncompleteSale;
            }
            set
            {
                if (this._selectedIncompleteSale == value)
                    return;
                this._selectedIncompleteSale = value;
                this.OnPropertyChanged(nameof(SelectedIncompleteSale));
            }
        }

        public DelegateCommand ProcessCommand { get; set; }

        public DelegateCommand SaveDetailsCommand { get; set; }

        public DelegateCommand ResertDetailsCommand { get; set; }

        public DelegateCommand PurchaseCommand { get; set; }

        public DelegateCommand SaleCommand { get; set; }

        public DelegateCommand ContainerCommand { get; set; }

        public bool IsConnected
        {
            get
            {
                return this._isConnected;
            }
            set
            {
                if (this._isConnected == value)
                    return;
                this._isConnected = value;
                this.OnPropertyChanged(nameof(IsConnected));
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

        public bool IsFirstWeighSaved
        {
            get
            {
                return this._isFirstWeighSaved;
            }
            set
            {
                if (this._isFirstWeighSaved == value)
                    return;
                this._isFirstWeighSaved = value;
                this.OnPropertyChanged(nameof(IsFirstWeighSaved));
            }
        }

        public ObservableCollection<Container> InCompleteContainers
        {
            get
            {
                return this._inCompleteContainers;
            }
            set
            {
                if (this._inCompleteContainers == value)
                    return;
                this._inCompleteContainers = value;
                this.OnPropertyChanged(nameof(InCompleteContainers));
            }
        }

        public void Dispose()
        {
            this.DeliveryViewModel.IsValidEvent -= new EventHandler(this.IsValidModelValidEventCheck);
            this.SupplyViewModel.IsValidEvent -= new EventHandler(this.IsValidModelValidEventCheck);
        }

        private void serialPortReader_ScaleValueEvent(object sender, SerialportEventArgs e)
        {
            this.WeighBridgeScale = e.SerialPortNumber;
        }

        private void IsValidModelValidEventCheck(object sender, EventArgs e)
        {
            if (this.Options.ToString() == "Sale")
                this.IsModelValid = this.DeliveryViewModel.IsValid;
            else
                this.IsModelValid = this.SupplyViewModel.IsValid;
        }

        private void ResertWeightBridge()
        {
            this.Initialize();
        }

        private void ProcessWeight()
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to process weight?", "Process weight", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;
                this.IsBusy = true;
                Options options;
                if (this._weighCounter == 0)
                {
                    this.FirstMass = this.WeighBridgeScale;
                    this.Caption = "Second Weight";
                    this._processStatus = "FirstWeight";
                    options = this.Options;
                    EventLogHelper.Log(string.Format("First weight of {0} Kg taken.Truck reg {1}", (object)this.FirstMass, options.ToString().ToUpper() == "PURCHASE" ? (object)this.SupplyViewModel.SelectedTruck.TruckRegNumber : (object)this.DeliveryViewModel.TruckRegNum));
                }
                if (this._weighCounter == 1)
                {
                    this.SecondMass = this.WeighBridgeScale;
                    this._processStatus = "UnProcessed";
                    this.GetNettMass();
                    options = this.Options;
                    EventLogHelper.Log(string.Format("Second weight of {0} Kg taken.Truck reg {1}", (object)this.SecondMass, options.ToString().ToUpper() == "PURCHASE" ? (object)this.SupplyViewModel.SelectedTruck.TruckRegNumber : (object)this.DeliveryViewModel.TruckRegNum));
                }
                this._weighCounter = this._weighCounter + 1;
                this.SaveToDataBase();
                this.IsBusy = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void GetNettMass()
        {
            this.NettMass = Math.Abs(this.FirstMass - this.SecondMass);
            this.ScaleNettMass = string.Format("{0} KG", (object)this.NettMass);
        }

        private void Initialize()
        {
            try
            {
                this.IsNewWeightSelected = true;
                this.Caption = "First weight";
                this.IsFirstWeighSaved = false;
                this._weighCounter = 0;
                this.SecondMass = 0L;
                this.FirstMass = 0L;
                this.NettMass = 0L;
                this.SupplyViewModel = new SupplyViewModel((Purchase)null);
                this.SupplyViewModel.IsValidEvent += new EventHandler(this.IsValidModelValidEventCheck);
                this.ProcessCommand = new DelegateCommand(new Action(this.ProcessWeight));
                this.SaveDetailsCommand = new DelegateCommand(new Action(this.SaveToDataBase));
                this.PurchaseCommand = new DelegateCommand(new Action(this.ProcessPurchase));
                this.ContainerCommand = new DelegateCommand(new Action(this.ProcessContainer));
                this.SaleCommand = new DelegateCommand(new Action(this.ProcessSale));
                this.ResertDetailsCommand = new DelegateCommand(new Action(this.ResertWeightBridge));
                this.GetIncompletePurchases();
                this.GetIncompleteContainers();
                this.GetIncompleteSales();
                this.SelectedIncompleteContainer = (Container)null;
                this.SelectedIncompletePurchase = (Purchase)null;
                SupplyView supplyView = new SupplyView();
                SupplyViewModel supplyViewModel = this.SupplyViewModel;
                supplyView.DataContext = (object)supplyViewModel;
                this.WeighOptionView = (UserControl)supplyView;
                this.IsModelValid = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void ProcessSale()
        {
            try
            {
                this.Options = Options.Sale;
                this.DeliveryViewModel = new DeliveryViewModel((Container)null, this.SelectedIncompleteSale);
                this.DeliveryViewModel.IsValidEvent += new EventHandler(this.IsValidModelValidEventCheck);
                this.DeliveryViewModel.IsValid = true;
                this.DeliveryViewModel.IsLocal = true;
                this.IsModelValid = true;
                DeliveryView deliveryView = new DeliveryView();
                DeliveryViewModel deliveryViewModel = this.DeliveryViewModel;
                deliveryView.DataContext = (object)deliveryViewModel;
                this.WeighOptionView = (UserControl)deliveryView;
                this.FirstMass = this.SelectedIncompleteSale.WeighBridgeInfo.FirstMass;
                this.IsNewWeightSelected = true;
                this._weighCounter = 1;
                this.Caption = "Second Weight";
            }
            catch (Exception ex)
            {
            }
        }

        private void ProcessContainer()
        {
            try
            {
                this.Options = Options.Sale;
                this.DeliveryViewModel = new DeliveryViewModel(this.SelectedIncompleteContainer, (Sale)null);
                this.DeliveryViewModel.IsValidEvent += new EventHandler(this.IsValidModelValidEventCheck);
                this.DeliveryViewModel.IsValid = true;
                this.IsModelValid = true;
                DeliveryView deliveryView = new DeliveryView();
                DeliveryViewModel deliveryViewModel = this.DeliveryViewModel;
                deliveryView.DataContext = (object)deliveryViewModel;
                this.WeighOptionView = (UserControl)deliveryView;
                this.FirstMass = this.SelectedIncompleteContainer.WeighBridgeInfo.FirstMass;
                this.IsNewWeightSelected = true;
                this._weighCounter = 1;
                this.Caption = "Second Weight";
            }
            catch (Exception ex)
            {
            }
        }

        private void ProcessPurchase()
        {
            try
            {
                this.Options = Options.Purchase;
                this.SupplyViewModel = new SupplyViewModel(this.SelectedIncompletePurchase);
                this.SupplyViewModel.IsValidEvent += new EventHandler(this.IsValidModelValidEventCheck);
                this.SupplyViewModel.IsValid = true;
                this.IsModelValid = true;
                SupplyView supplyView = new SupplyView();
                SupplyViewModel supplyViewModel = this.SupplyViewModel;
                supplyView.DataContext = (object)supplyViewModel;
                this.WeighOptionView = (UserControl)supplyView;
                this.FirstMass = this.SelectedIncompletePurchase.WeighBridgeInfo.FirstMass;
                this.IsNewWeightSelected = true;
                this._weighCounter = 1;
                this.Caption = "Second Weight";
            }
            catch (Exception ex)
            {
            }
        }

        private void GetIncompletePurchases()
        {
            try
            {
                if (!Program.IsServiceDown)
                {
                    using (DataClassService dataClassService = new DataClassService())
                    {
                        InCompletePurchases = new ObservableCollection<Purchase>();
                        IEnumerable<Purchase> incompletePurchases = dataClassService.GetIncompletePurchases();
                        foreach (Purchase purchase in incompletePurchases)
                            InCompletePurchases.Add(purchase);
                        if (incompletePurchases.Count() <= 0)
                            return;
                        using (WeighModeJson weighModeJson = new WeighModeJson())
                        {
                            Purchase purchase = incompletePurchases.OrderByDescending(p => p.Id).FirstOrDefault();
                            weighModeJson.UpdateWeigbridgeFile(purchase.Id, purchase.WeighBridgeInfo.Id, 0);
                        }
                    }
                }
                else
                {
                    using (WeighModeJson weighModeJson = new WeighModeJson())
                    {
                        this.InCompletePurchases = new ObservableCollection<Purchase>();
                        foreach (Purchase incompletePurchase in weighModeJson.GetIncompletePurchases())
                            this.InCompletePurchases.Add(incompletePurchase);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void GetIncompleteSales()
        {
            try
            {
                if (!Program.IsServiceDown)
                {
                    using (DataClassService dataClassService = new DataClassService())
                    {
                        this.InCompleteSales = new ObservableCollection<Sale>();
                        IEnumerable<Sale> incompleteSales = dataClassService.GetIncompleteSales();
                        foreach (Sale sale in incompleteSales)
                            this.InCompleteSales.Add(sale);
                        if (incompleteSales.Count<Sale>() <= 0)
                            return;
                        using (WeighModeJson weighModeJson = new WeighModeJson())
                        {
                            Sale sale = incompleteSales.OrderByDescending<Sale, int>((Func<Sale, int>)(p => p.Id)).FirstOrDefault<Sale>();
                            weighModeJson.UpdateWeigbridgeFile(0, sale.WeighBridgeInfo.Id, sale.Id);
                        }
                    }
                }
                else
                {
                    using (WeighModeJson weighModeJson = new WeighModeJson())
                    {
                        this.InCompleteSales = new ObservableCollection<Sale>();
                        foreach (Sale incompleteSale in weighModeJson.GetIncompleteSales())
                            this.InCompleteSales.Add(incompleteSale);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void GetIncompleteContainers()
        {
            if (!Program.IsServiceDown)
            {
                using (DataClassService dataClassService = new DataClassService())
                {
                    this.InCompleteContainers = new ObservableCollection<Container>();
                    foreach (Container incompleteContainer in dataClassService.GetIncompleteContainers())
                        this.InCompleteContainers.Add(incompleteContainer);
                }
            }
            else
            {
                using (WeighModeJson weighModeJson = new WeighModeJson())
                {
                    this.InCompleteContainers = new ObservableCollection<Container>();
                    foreach (Container incompleteContainer in weighModeJson.GetIncompleteContainers())
                        this.InCompleteContainers.Add(incompleteContainer);
                }
            }
        }

        private void SaveToDataBase()
        {
            if (this.Options.ToString() == "Purchase")
                this.SavePurchaseToDatabase();
            else
                this.SaveSaleToDatabase();
        }

        private void SaveSaleToDatabase()
        {
            if (this.SelectedIncompleteContainer == null && this.SelectedIncompleteSale == null)
            {
                WeighBridgeInfo weighBridgeInfo = new WeighBridgeInfo()
                {
                    FirstMass = this.FirstMass,
                    DateIn = DateTime.Now,
                    Product = this.DeliveryViewModel.ProductType,
                    Comments = this.DeliveryViewModel.ExtraInfo
                };
                if (!this.DeliveryViewModel.IsLocal)
                {
                    Container container = new Container()
                    {
                        ContainerNumber = this.DeliveryViewModel.SelectedContainer.ContainerNumber,
                        Sealnumber = this.DeliveryViewModel.SealNumber,
                        Product = this.DeliveryViewModel.ProductType,
                        TruckRegNumber = this.DeliveryViewModel.TruckRegNum,
                        WeighBridgeInfo = weighBridgeInfo,
                        Status = this._processStatus
                    };
                    using (DataClassService dataClassService = new DataClassService())
                        dataClassService.AddContainerWeight(container);
                }
                else
                {
                    Customer customer = new Customer()
                    {
                        Id = this.DeliveryViewModel.SelectedCustomer.Id
                    };
                    Sale sale = new Sale()
                    {
                        ExtraInfo = this.DeliveryViewModel.ExtraInfo,
                        TruckRegNumber = this.DeliveryViewModel.TruckRegNum,
                        WeighBridgeInfo = weighBridgeInfo,
                        Status = this._processStatus,
                        Customer = customer
                    };
                    if (!Program.IsServiceDown)
                    {
                        using (DataClassService dataClassService = new DataClassService())
                            dataClassService.AddSale(sale);
                    }
                    else
                    {
                        using (WeighModeJson weighModeJson = new WeighModeJson())
                            weighModeJson.AddSale(sale);
                    }
                }
            }
            else if (!this.DeliveryViewModel.IsLocal)
            {
                long nettWight = Math.Abs(this.NettMass - this.SelectedIncompleteContainer.TareWeight);
                WeighBridgeInfo weighBridgeInfo = new WeighBridgeInfo()
                {
                    FirstMass = this.SelectedIncompleteContainer.WeighBridgeInfo.FirstMass,
                    SecondMass = this.SecondMass,
                    DateIn = this.SelectedIncompleteContainer.WeighBridgeInfo.DateIn,
                    DateOut = DateTime.Now,
                    NettMass = nettWight
                };
                Container container = new Container()
                {
                    ContainerNumber = this.SelectedIncompleteContainer.ContainerNumber,
                    Sealnumber = this.SelectedIncompleteContainer.Sealnumber,
                    Product = this.DeliveryViewModel.ProductType,
                    TruckRegNumber = this.SelectedIncompleteContainer.TruckRegNumber ?? this.DeliveryViewModel.TruckRegNum,
                    WeighBridgeInfo = weighBridgeInfo,
                    Status = this._processStatus
                };
                using (DataClassService dataClassService = new DataClassService())
                    dataClassService.UpdateContainer(container, nettWight);
                this.SelectedIncompleteContainer.WeighBridgeInfo.NettMass = nettWight;
                this.SelectedIncompleteContainer.TruckRegNumber = this.SelectedIncompleteContainer.TruckRegNumber ?? this.DeliveryViewModel.TruckRegNum;
                this.SelectedIncompleteContainer.WeighBridgeInfo.SecondMass = this.SecondMass;
                this.SelectedIncompleteContainer.WeighBridgeInfo.DateOut = DateTime.Now;
                this.PrintContainerReceipt(this.SelectedIncompleteContainer, this.Options.ToString().ToUpper(), this.DeliveryViewModel.SelectedCustomer);
            }
            else
            {
                WeighBridgeInfo weighBridgeInfo = new WeighBridgeInfo()
                {
                    FirstMass = this.SelectedIncompleteSale.WeighBridgeInfo.FirstMass,
                    SecondMass = this.SecondMass,
                    DateIn = this.SelectedIncompleteSale.WeighBridgeInfo.DateIn,
                    DateOut = DateTime.Now,
                    NettMass = this.NettMass,
                    Product = this.DeliveryViewModel.SelectedProduct.Name
                };
                Customer customer = new Customer()
                {
                    Id = this.DeliveryViewModel.SelectedCustomer.Id
                };
                Sale sale = new Sale()
                {
                    ExtraInfo = this.DeliveryViewModel.ExtraInfo,
                    TruckRegNumber = this.DeliveryViewModel.TruckRegNum,
                    WeighBridgeInfo = weighBridgeInfo,
                    Status = this._processStatus,
                    Id = this.SelectedIncompleteSale.Id,
                    Customer = customer
                };
                if (!Program.IsServiceDown)
                {
                    using (DataClassService dataClassService = new DataClassService())
                        dataClassService.UpdateSale(sale);
                }
                else
                {
                    using (WeighModeJson weighModeJson = new WeighModeJson())
                        weighModeJson.UpdateSale(sale);
                }
                this.SelectedIncompleteSale.WeighBridgeInfo.SecondMass = this.SecondMass;
                this.SelectedIncompleteSale.WeighBridgeInfo.NettMass = this.NettMass;
                this.PrintSaleReceipt(this.SelectedIncompleteSale, "SALE");
            }
            this.Initialize();
        }

        private void SavePurchaseToDatabase()
        {

            if (this.SelectedIncompletePurchase == null)
            {
                Driver driver = new Driver()
                {
                    Id = this.SupplyViewModel.SelectedDriver.Id,
                    Firstname = this.SupplyViewModel.SelectedDriver.Firstname,
                    Surname = this.SupplyViewModel.SelectedDriver.Surname,
                    IdNumber = this.SupplyViewModel.SelectedDriver.IdNumber
                };
                Truck truck1 = new Truck();
                truck1.Id = this.SupplyViewModel.SelectedTruck.Id;
                truck1.TruckRegNumber = this.SupplyViewModel.SelectedTruck.TruckRegNumber;
                int num = this.SupplyViewModel.SelectedTruck.Own ? 1 : 0;
                truck1.Own = num != 0;
                Truck truck2 = truck1;

                WeighBridgeInfo weighBridgeInfo = new WeighBridgeInfo()
                {
                    FirstMass = this.FirstMass,
                    DateIn = DateTime.Now,
                    Product = this.SupplyViewModel.SelectedProduct.Name,
                    Comments = this.SupplyViewModel.Comments
                };
                if (this._processStatus == "FirstWeight")
                {
                    Purchase purchase = new Purchase();
                    purchase.Driver = driver;
                    purchase.Truck = truck2;
                    purchase.WeighBridgeInfo = weighBridgeInfo;
                    string processStatus = this._processStatus;
                    purchase.Status = processStatus;
                    this._purchase = purchase;


                    if (!Program.IsServiceDown)
                    {
                        using (DataClassService dataClassService = new DataClassService())
                        {
                            dataClassService.AddPurchase(this._purchase);
                            this.IsFirstWeighSaved = true;
                        }
                    }
                    else
                    {
                        using (WeighModeJson weighModeJson = new WeighModeJson())
                        {
                            weighModeJson.AddPurchase(this._purchase);
                            this.IsFirstWeighSaved = true;
                        }
                    }
                }
            }
            else
            {
                this.SelectedIncompletePurchase.WeighBridgeInfo.SecondMass = this.SecondMass;
                this.SelectedIncompletePurchase.WeighBridgeInfo.DateOut = DateTime.Now;
                this.SelectedIncompletePurchase.WeighBridgeInfo.Comments = this.SupplyViewModel.Comments;
                this.SelectedIncompletePurchase.WeighBridgeInfo.Product = this.SupplyViewModel.SelectedProduct.Name;
                this.SelectedIncompletePurchase.WeighBridgeInfo.NettMass = this.NettMass;
                this.SelectedIncompletePurchase.Price = this.SupplyViewModel.SelectedSupplier.SupplierProducts.FirstOrDefault<SupplierProduct>((Func<SupplierProduct, bool>)(sp => sp.ProductId == this.SupplyViewModel.SelectedProduct.Id)) != null ? this.SupplyViewModel.SelectedSupplier.SupplierProducts.FirstOrDefault<SupplierProduct>((Func<SupplierProduct, bool>)(sp => sp.ProductId == this.SupplyViewModel.SelectedProduct.Id)).SupplierPrice : this.SupplyViewModel.SelectedProduct.RustiviaPrice;
                this.SelectedIncompletePurchase.TotalPrice = (Decimal)this.NettMass * this.SelectedIncompletePurchase.Price;
                WeighBridgeInfo weighBridgeInfo = new WeighBridgeInfo()
                {
                    Id = this.SelectedIncompletePurchase.WeighBridgeInfo.Id,
                    SecondMass = this.SecondMass,
                    DateOut = DateTime.Now,
                    NettMass = this.NettMass,
                    Product = this.SupplyViewModel.SelectedProduct.Name,
                    Comments = this.SupplyViewModel.Comments
                };
                Purchase purchase = new Purchase()
                {
                    Id = this.SelectedIncompletePurchase.Id,
                    WeighBridgeInfo = weighBridgeInfo,
                    Status = this._processStatus,
                    Price = this.SelectedIncompletePurchase.Price,
                    TotalPrice = this.SelectedIncompletePurchase.TotalPrice
                };
                if (!Program.IsServiceDown)
                {
                    using (DataClassService dataClassService = new DataClassService())
                        dataClassService.UpdatePurchase(purchase);
                }
                else
                {
                    using (WeighModeJson weighModeJson = new WeighModeJson())
                        weighModeJson.UpdatePurchase(purchase);
                }
                this.ViewBaseProduct = this.SupplyViewModel.SelectedProduct;
                this.ViewBaseSupplier = this.SupplyViewModel.SelectedSupplier;
                this.PrintWeightBridgeReceipt(this.SelectedIncompletePurchase, this.ViewBaseSupplier, this.Options.ToString().ToUpper());
            }
            this.Initialize();
        }
    }
}
