// Decompiled with JetBrains decompiler
// Type: WeighBridgeApplication.ViewModel.SupplyViewModel
// Assembly: WeighBridgeApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0008ECA4-ECDC-4057-A6F3-F7FD4CE2F23F
// Assembly location: C:\Users\Ndavhe\Desktop\Rustivia Weight Bridge\WeighBridgeApplication.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WeighBridgeApplication.Model;

namespace WeighBridgeApplication.ViewModel
{
    public class SupplyViewModel : ViewModelBase
    {
        private string _comments;
        private ReadOnlyObservableCollection<Driver> _driverList;
        private bool _isOwn;
        private bool _isValid;
        private ReadOnlyObservableCollection<Product> _productList;
        private Driver _selectedDriver;
        private Product _selectedProduct;
        private Supplier _selectedSupplier;
        private Truck _selectedTruck;
        private ReadOnlyObservableCollection<Supplier> _supplierList;
        private ReadOnlyObservableCollection<Truck> _truckList;

        public SupplyViewModel(Purchase purchase = null)
        {
            this.Trucks = new ReadOnlyObservableCollection<Truck>(this.GetTrucks());
            this.Suppliers = new ReadOnlyObservableCollection<Supplier>(this.GetSuppliers());
            this.SupplierList = this.Suppliers;
            this.DriverList = new ReadOnlyObservableCollection<Driver>(this.GetSuppliersDrivers((IEnumerable<Supplier>)this.Suppliers));
            this.TruckList = new ReadOnlyObservableCollection<Truck>(this.GetSuppliersTrucks((IEnumerable<Supplier>)this.Suppliers));
            this.ProductList = new ReadOnlyObservableCollection<Product>(this.GetProducts());
            if (purchase == null)
                return;
            Driver driver = this.DriverList.SingleOrDefault<Driver>((Func<Driver, bool>)(d => d.Id == purchase.Driver.Id));
            this.SelectedSupplier = this.SupplierList.FirstOrDefault<Supplier>((Func<Supplier, bool>)(s =>
            {
                if (driver != null)
                    return s.Id == driver.SupplierInfo.Id;
                return false;
            }));
            this.IsOwn = purchase.Truck.Own;
            this.SelectedDriver = this.DriverList.FirstOrDefault<Driver>((Func<Driver, bool>)(d => d.Id == purchase.Driver.Id));
            this.SelectedTruck = this.TruckList.FirstOrDefault<Truck>((Func<Truck, bool>)(t => t.Id == purchase.Truck.Id));
            this.Comments = purchase.WeighBridgeInfo.Comments;
            this.SelectedProduct = this.ProductList.FirstOrDefault<Product>((Func<Product, bool>)(pr => pr.Name == purchase.WeighBridgeInfo.Product));
        }

        public ReadOnlyObservableCollection<Truck> TruckList
        {
            get
            {
                return this._truckList;
            }
            set
            {
                if (this._truckList == value)
                    return;
                this._truckList = value;
                this.OnPropertyChanged(nameof(TruckList));
            }
        }

        public ReadOnlyObservableCollection<Supplier> SupplierList
        {
            get
            {
                return this._supplierList;
            }
            set
            {
                if (this._supplierList == value)
                    return;
                this._supplierList = value;
                this.OnPropertyChanged(nameof(SupplierList));
            }
        }

        public ReadOnlyObservableCollection<Driver> DriverList
        {
            get
            {
                return this._driverList;
            }
            set
            {
                if (this._driverList == value)
                    return;
                this._driverList = value;
                this.OnPropertyChanged(nameof(DriverList));
            }
        }

        public ReadOnlyObservableCollection<Product> ProductList
        {
            get
            {
                return this._productList;
            }
            set
            {
                this._productList = value;
                this.OnPropertyChanged(nameof(ProductList));
            }
        }

        public Product SelectedProduct
        {
            get
            {
                return this._selectedProduct;
            }
            set
            {
                if (this._selectedProduct == value)
                    return;
                this._selectedProduct = value;
                this.OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        public string Comments
        {
            get
            {
                return this._comments;
            }
            set
            {
                if (!(this._comments != value))
                    return;
                this._comments = value;
                this.OnPropertyChanged(nameof(Comments));
            }
        }

        public Driver SelectedDriver
        {
            get
            {
                return this._selectedDriver;
            }
            set
            {
                if (this._selectedDriver == value)
                    return;
                this._selectedDriver = value;
                this.IsModelValid();
                this.OnPropertyChanged(nameof(SelectedDriver));
            }
        }

        public Supplier SelectedSupplier
        {
            get
            {
                return this._selectedSupplier;
            }
            set
            {
                if (this._selectedSupplier == value)
                    return;
                this._selectedSupplier = value;
                if (this._selectedSupplier != null)
                {
                    this.DriverList = new ReadOnlyObservableCollection<Driver>(this.GetSupplierDrivers(this._selectedSupplier.Id));
                    this.TruckList = this.IsOwn ? new ReadOnlyObservableCollection<Truck>(this.GetOwnTrucks()) : new ReadOnlyObservableCollection<Truck>(this.GetSupplierTrucks(this._selectedSupplier.Id));
                }
                this.IsModelValid();
                this.OnPropertyChanged(nameof(SelectedSupplier));
            }
        }

        public Truck SelectedTruck
        {
            get
            {
                return this._selectedTruck;
            }
            set
            {
                if (this._selectedTruck == value)
                    return;
                this._selectedTruck = value;
                this.IsModelValid();
                this.OnPropertyChanged(nameof(SelectedTruck));
            }
        }

        public bool IsValid
        {
            get
            {
                return this._isValid;
            }
            set
            {
                if (this._isValid == value)
                    return;
                this._isValid = value;
                this.OnPropertyChanged(nameof(IsValid));
            }
        }

        public bool IsOwn
        {
            get
            {
                return this._isOwn;
            }
            set
            {
                if (this._isOwn == value)
                    return;
                this._isOwn = value;
                this.TruckList = this.IsOwn ? new ReadOnlyObservableCollection<Truck>(this.GetOwnTrucks()) : this.Trucks;
                this.OnPropertyChanged(nameof(IsOwn));
            }
        }

        public event EventHandler IsValidEvent;

        private ObservableCollection<Driver> GetSuppliersDrivers(IEnumerable<Supplier> suppliers)
        {
            ObservableCollection<Driver> observableCollection = new ObservableCollection<Driver>();
            foreach (Supplier supplier in suppliers)
            {
                foreach (Driver driver in supplier.Drivers)
                {
                    driver.FullName = string.Format("{0} {1}", (object)driver.Firstname, (object)driver.Surname);
                    driver.SupplierInfo = supplier;
                    observableCollection.Add(driver);
                }
            }
            return observableCollection;
        }

        private ObservableCollection<Truck> GetSuppliersTrucks(IEnumerable<Supplier> suppliers)
        {
            ObservableCollection<Truck> observableCollection = new ObservableCollection<Truck>();
            foreach (Supplier supplier in suppliers)
            {
                foreach (Truck truck in supplier.Trucks)
                    observableCollection.Add(truck);
            }
            return observableCollection;
        }

        private ObservableCollection<Truck> GetOwnTrucks()
        {
            ObservableCollection<Truck> observableCollection = new ObservableCollection<Truck>();
            foreach (Truck truck in this.Trucks.Where<Truck>((Func<Truck, bool>)(t => t.Own)).ToList<Truck>())
                observableCollection.Add(truck);
            return observableCollection;
        }

        private void IsModelValid()
        {
            this.IsValid = this.SelectedDriver != null && this.SelectedSupplier != null && this.SelectedTruck != null;
            // ISSUE: reference to a compiler-generated field
            if (this.IsValidEvent == null)
                return;
            // ISSUE: reference to a compiler-generated field
            this.IsValidEvent((object)this, new EventArgs());
        }

        private ObservableCollection<Truck> GetSupplierTrucks(int supplierId)
        {
            ObservableCollection<Truck> observableCollection = new ObservableCollection<Truck>();
            foreach (Truck truck in this.Suppliers.Single<Supplier>((Func<Supplier, bool>)(s => s.Id == supplierId)).Trucks.ToList<Truck>())
                observableCollection.Add(truck);
            return observableCollection;
        }

        private ObservableCollection<Driver> GetSupplierDrivers(int supplierId)
        {
            ObservableCollection<Driver> observableCollection = new ObservableCollection<Driver>();
            foreach (Driver driver in this.Suppliers.Single<Supplier>((Func<Supplier, bool>)(s => s.Id == supplierId)).Drivers.ToList<Driver>())
                observableCollection.Add(new Driver()
                {
                    Id = driver.Id,
                    Firstname = driver.Firstname,
                    Surname = driver.Surname,
                    FullName = string.Format("{0} {1}", (object)driver.Firstname, (object)driver.Surname),
                    IdNumber = driver.IdNumber,
                    IdLocation = driver.IdNumber
                });
            return observableCollection;
        }
    }
}
