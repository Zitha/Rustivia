using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using WeighBridgeApplication.Model;
using WeighBridgeApplication.RustiviaService.DbContext;
using WeighBridgeApplication.Util;
using Driver = WeighBridgeApplication.Model.Driver;
using Truck = WeighBridgeApplication.Model.Truck;

namespace WeighBridgeApplication.ViewModel
{
    public class ViewModelBase : PropertyChangedNotify
    {
        protected readonly DataContextClient ContextClient = new DataContextClient();
        private ReadOnlyObservableCollection<Driver> _drivers;
        private ReadOnlyObservableCollection<Supplier> _suppliers;
        private ReadOnlyObservableCollection<Truck> _trucks;

        protected ViewModelBase()
        {
            Trucks = new ReadOnlyObservableCollection<Truck>(GetTrucks());
            Drivers = new ReadOnlyObservableCollection<Driver>(GetDrivers());
            Suppliers = new ReadOnlyObservableCollection<Supplier>(GetSuppliers());
        }

        protected ReadOnlyObservableCollection<Truck> Trucks
        {
            get { return _trucks; }
            set
            {
                if (_trucks != value)
                {
                    _trucks = value;
                    OnPropertyChanged();
                }
            }
        }

        protected ReadOnlyObservableCollection<Driver> Drivers
        {
            get { return _drivers; }
            set
            {
                if (_drivers != value)
                {
                    _drivers = value;
                    OnPropertyChanged();
                }
            }
        }

        protected ReadOnlyObservableCollection<Supplier> Suppliers
        {
            get { return _suppliers; }
            set
            {
                if (_suppliers != value)
                {
                    _suppliers = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Truck> GetTrucks()
        {
            TruckDto[] trucksDto = ContextClient.GetTrucksAsync().Result;
            var trucks = new ObservableCollection<Truck>();
            foreach (TruckDto truckDto in trucksDto)
            {
                trucks.Add(new Truck
                {
                    Id = truckDto.Id,
                    TruckRegNumber = truckDto.TruckRegNumber
                });
            }
            return trucks;
        }

        private ObservableCollection<Supplier> GetSuppliers()
        {
            SupplierInfoDto[] supplierInfoDto = ContextClient.GetSuppliersAsync().Result;
            var suppliers = new ObservableCollection<Supplier>();
            foreach (SupplierInfoDto supplier in supplierInfoDto)
            {
                suppliers.Add(new Supplier
                {
                    Id = supplier.Id,
                    SupplierName = supplier.SupplierName,
                    Suppliercode = supplier.Suppliercode,
                    Drivers = GetDrivers(supplier.Drivers),
                    Trucks = GetTrucks(supplier.Trucks)
                });
            }
            return suppliers;
        }

        private ObservableCollection<Driver> GetDrivers()
        {
            DriverDto[] driversDto = ContextClient.GetDriversAsync().Result;
            var drivers = new ObservableCollection<Driver>();
            foreach (DriverDto driver in driversDto)
            {
                drivers.Add(new Driver
                {
                    Id = driver.Id,
                    Firstname = driver.Firstname,
                    Surname = driver.Surname,
                    FullName = string.Format("{0} {1}", driver.Firstname, driver.Surname),
                    IdNumber = driver.IdNumber,
                    IdLocation = driver.IdNumber,
                    SupplierInfo = GetSupplier(driver.SupplierInfo)
                });
            }
            return drivers;
        }

        private Supplier GetSupplier(SupplierInfoDto supplier)
        {
            return new Supplier
            {
                Id = supplier.Id,
                SupplierName = supplier.SupplierName,
                Suppliercode = supplier.Suppliercode,
                Drivers = GetDrivers(supplier.Drivers),
                Trucks = GetTrucks(supplier.Trucks)
            };
        }

        private List<Driver> GetDrivers(IEnumerable<DriverDto> drivers)
        {
            return drivers.Select(driver => new Driver
            {
                Id = driver.Id,
                Firstname = driver.Firstname,
                Surname = driver.Surname,
                IdNumber = driver.IdNumber
            }).ToList();
        }

        private List<Truck> GetTrucks(IEnumerable<TruckDto> trucks)
        {
            return trucks.Select(truck => new Truck
            {
                Id = truck.Id,
                TruckRegNumber = truck.TruckRegNumber
            }).ToList();
        }

        protected RustiviaService.DbContext.Truck GetTruck(TruckDto driver)
        {
            return new RustiviaService.DbContext.Truck
            {
                Id = driver.Id,
                TruckRegNumber = driver.TruckRegNumber
            };
        }

        public RustiviaService.DbContext.Driver GetDriver(DriverDto driver)
        {
            return new RustiviaService.DbContext.Driver
            {
                Id = driver.Id,
                Firstname = driver.Firstname,
                Surname = driver.Surname,
                IdNumber = driver.IdNumber,
                //                SupplierInfo=driver.SupplierInfo
            };
        }

        protected void PrintReceipt(WeighBridgeInfo weighBridgeInfo)
        {
            Driver supplierInfoDriver = Drivers.SingleOrDefault(a => a.Id == weighBridgeInfo.Driver.Id);
            Supplier supplierInfo = null;
            if (supplierInfoDriver != null)
            {
                supplierInfo = supplierInfoDriver.SupplierInfo;
            }
            var printDialog = new PrintDialog();

            if (printDialog.ShowDialog().GetValueOrDefault())
            {
                var flowDoc = new FlowDocument {PageWidth = 320f};

                //Header
                var header = new Paragraph
                {
                    Margin = new Thickness(0),
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 18,
                    Padding = new Thickness(1, 0, 1, 0)
                };
                header.Inlines.Add("Rustivia Metals CC");
                flowDoc.Blocks.Add(header);

                var para = new Paragraph
                {
                    Margin = new Thickness(0),
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 12,
                    Padding = new Thickness(1, 3, 1, 0)
                };
                //Address
                var address = new Paragraph
                {
                    Margin = new Thickness(0),
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 12,
                    Padding = new Thickness(1, 0, 1, 0)
                };
                address.Inlines.Add("54 Northreef");
                address.Inlines.Add(new LineBreak());
                address.Inlines.Add("Activia perk");
                address.Inlines.Add(new LineBreak());
                address.Inlines.Add("Germiston, South Africa");
                address.Inlines.Add(new LineBreak());
                address.Inlines.Add("(T) 011 828 9961");
                address.Inlines.Add(new LineBreak());
                address.Inlines.Add("Reg no. 1997/0025504/23");
                address.Inlines.Add(new LineBreak());
                address.Inlines.Add("Vat no. 4610216634");
                address.Inlines.Add(new LineBreak());
                var pLine = new Line
                {
                    Stretch = Stretch.Fill,
                    Stroke = Brushes.Black,
                    X2 = 1,
                    Margin = new Thickness(0, 0, 30, 0)
                };
                address.Inlines.Add(pLine);
                flowDoc.Blocks.Add(address);

                //Transaction
                string tranIdDescription = "Transaction No:".PadRight(17);
                string tranId = weighBridgeInfo.Id.ToString(CultureInfo.InvariantCulture);
                string tranLine = tranIdDescription + tranId;

                //Registration
                string regNoDescription = "Registration No:".PadRight(18);
                string regNo = weighBridgeInfo.Truck.TruckRegNumber;
                string regLine = regNoDescription + regNo;

                //Date
                string dateDescription = "Date:".PadRight(24);
                string date = weighBridgeInfo.Date.ToString("yyyy-MM-dd");
                string dateLine = dateDescription + date;

                //Supplier
                string supplierDescription = "Supplier: ".PadRight(23);
                string supplier = supplierInfo != null ? supplierInfo.SupplierName : null;
                string supplierLine = (supplierDescription + supplier);

                //Driver
                string driverDescription = "Driver:".PadRight(23);
                string driver = string.Format("{0} {1}", weighBridgeInfo.Driver.Surname,
                    weighBridgeInfo.Driver.Firstname);
                string driverLine = (driverDescription + driver);

                //Product
                string productDescription = "Product:".PadRight(22);
                string product = weighBridgeInfo.Product;
                string productLine = (productDescription + product);

                //Comments
                string commentsDescription = "Comments:".PadRight(18);
                string comments = weighBridgeInfo.Comments;
                string commentsLine = (commentsDescription + comments);

                //First Mass
                string firstmassDescription = "First Mass:".PadRight(22);
                string firstmass = string.Format("{0} KG",
                    weighBridgeInfo.FirstMass.ToString(CultureInfo.InvariantCulture));
                string firstmassLine = (firstmassDescription + firstmass);

                //First Mass
                string secondmassDescription = "Second Mass:".PadRight(18);
                string secondmass = string.Format("{0} KG",
                    weighBridgeInfo.SecondMass.ToString(CultureInfo.InvariantCulture));
                string secondmassLine = (secondmassDescription + secondmass);

                //First Mass
                string nettmassDescription = "Nett Mass:".PadRight(22);
                string nettmass = string.Format("{0} KG",
                    weighBridgeInfo.NettMass.ToString(CultureInfo.InvariantCulture));
                string nettmassLine = (nettmassDescription + nettmass);

                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new Run(tranLine));
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new Run(regLine));
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new Run(dateLine));
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new Run(supplierLine));
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new Run(driverLine));
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new Run(productLine));
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new Run(commentsLine));
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new Run(firstmassLine));
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new Run(secondmassLine));
                para.Inlines.Add(new LineBreak());
                para.Inlines.Add(new LineBreak());
                var pLine2 = new Line
                {
                    Stretch = Stretch.Fill,
                    Stroke = Brushes.Black,
                    X2 = 1,
                    Margin = new Thickness(0, 0, 10, 0),
                };
                para.Inlines.Add(pLine2);
                para.Inlines.Add(new Run(nettmassLine));
                para.Inlines.Add(new LineBreak());
                var pLine3 = new Line
                {
                    Stretch = Stretch.Fill,
                    Stroke = Brushes.Black,
                    X2 = 1,
                    Margin = new Thickness(0, 40, 50, 0),
                };
                para.Inlines.Add(pLine3);
                para.Inlines.Add(new Run("Weighbridge Ckerk:"));
                para.Inlines.Add(new LineBreak());
                var pLine4 = new Line
                {
                    Stretch = Stretch.Fill,
                    Stroke = Brushes.Black,
                    X2 = 1,
                    Margin = new Thickness(0, 40, 80, 0),
                };
                para.Inlines.Add(pLine4);
                para.Inlines.Add(new Run("Driver signature:"));


                flowDoc.Blocks.Add(para);

                DocumentPaginator paginator =
                    (flowDoc as IDocumentPaginatorSource).DocumentPaginator;

                printDialog.PrintDocument(paginator, "Invoice");
            }
        }
    }
}