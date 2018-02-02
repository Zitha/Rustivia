using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WeighBridgeApplication.Model;
using WeighBridgeApplication.RustiviaService.DbContext;
using Driver = WeighBridgeApplication.Model.Driver;
using Truck = WeighBridgeApplication.Model.Truck;

namespace WeighBridgeApplication.ViewModel
{
    public class SupplyViewModel : ViewModelBase
    {
        private string _comments;
        private ReadOnlyObservableCollection<Driver> _driverList;
        private bool _isValid;
        private string _productType;
        private Driver _selectedDriver;
        private Supplier _selectedSupplier;
        private Truck _selectedTruck;
        private ReadOnlyObservableCollection<Supplier> _supplierList;
        private ReadOnlyObservableCollection<Truck> _truckList;

        public SupplyViewModel(WeighBridgeInfo weighBridge = null)
        {
            DriverList = Drivers;
            TruckList = Trucks;
            SupplierList = Suppliers;
            if (weighBridge != null)
            {
                Driver driver = DriverList.SingleOrDefault(d => d.Id == weighBridge.Driver.Id);
                SelectedSupplier = SupplierList.SingleOrDefault(s => driver != null && s.Id == driver.SupplierInfo.Id);
                SelectedDriver = DriverList.SingleOrDefault(d => d.Id == weighBridge.Driver.Id);
                SelectedTruck = TruckList.SingleOrDefault(t => t.Id == weighBridge.Truck.Id);
                Comments = weighBridge.Comments;
                ProductType = weighBridge.Product;
            }
        }

        public ReadOnlyObservableCollection<Truck> TruckList
        {
            get { return _truckList; }
            set
            {
                if (_truckList != value)
                {
                    _truckList = value;
                    OnPropertyChanged();
                }
            }
        }

        public ReadOnlyObservableCollection<Supplier> SupplierList
        {
            get { return _supplierList; }
            set
            {
                if (_supplierList != value)
                {
                    _supplierList = value;
                    OnPropertyChanged();
                }
            }
        }

        public ReadOnlyObservableCollection<Driver> DriverList
        {
            get { return _driverList; }
            set
            {
                if (_driverList != value)
                {
                    _driverList = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Comments
        {
            get { return _comments; }
            set
            {
                if (_comments != value)
                {
                    _comments = value;
                    OnPropertyChanged();
                }
            }
        }

        public Driver SelectedDriver
        {
            get { return _selectedDriver; }
            set
            {
                if (_selectedDriver != value)
                {
                    _selectedDriver = value;
                    IsModelValid();
                    OnPropertyChanged();
                }
            }
        }

        public Supplier SelectedSupplier
        {
            get { return _selectedSupplier; }
            set
            {
                if (_selectedSupplier != value)
                {
                    _selectedSupplier = value;
                    DriverList = new ReadOnlyObservableCollection<Driver>(GetSupplierDrivers(_selectedSupplier.Id));
                    TruckList = new ReadOnlyObservableCollection<Truck>(GetSupplierTrucks(_selectedSupplier.Id));
                    IsModelValid();
                    OnPropertyChanged();
                }
            }
        }

        public Truck SelectedTruck
        {
            get { return _selectedTruck; }
            set
            {
                if (_selectedTruck != value)
                {
                    _selectedTruck = value;
                    IsModelValid();
                    OnPropertyChanged();
                }
            }
        }

        public string ProductType
        {
            get { return _productType; }
            set
            {
                if (_productType != value)
                {
                    _productType = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Error { get; private set; }

        private void IsModelValid()
        {
            IsValid = SelectedDriver != null && SelectedSupplier != null && SelectedTruck != null;
        }

        private ObservableCollection<Truck> GetSupplierTrucks(int supplierId)
        {
            var trucks = new ObservableCollection<Truck>();
            List<Truck> supplierTrucks =
                Suppliers.Single(s => s.Id == supplierId).Trucks.ToList();
            foreach (Truck truck in supplierTrucks)
            {
                trucks.Add(truck);
            }
            return trucks;
        }

        private ObservableCollection<Driver> GetSupplierDrivers(int supplierId)
        {
            var drivers = new ObservableCollection<Driver>();
            List<Driver> supplierDrivers =
                Suppliers.Single(s => s.Id == supplierId).Drivers.ToList();
            foreach (Driver driver in supplierDrivers)
            {
                drivers.Add(new Driver
                {
                    Id = driver.Id,
                    Firstname = driver.Firstname,
                    Surname = driver.Surname,
                    FullName = string.Format("{0} {1}", driver.Firstname, driver.Surname),
                    IdNumber = driver.IdNumber,
                    IdLocation = driver.IdNumber
                });
            }
            return drivers;
        }
    }
}