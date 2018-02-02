// Decompiled with JetBrains decompiler
// Type: WeighBridgeApplication.ViewModel.ReprintInvoiceViewModel
// Assembly: WeighBridgeApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0008ECA4-ECDC-4057-A6F3-F7FD4CE2F23F
// Assembly location: C:\Users\Ndavhe\Desktop\Rustivia Weight Bridge\WeighBridgeApplication.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WeighBridgeApplication.Model;
using WeighBridgeApplication.Util;

namespace WeighBridgeApplication.ViewModel
{
    public class ReprintInvoiceViewModel : ViewModelBase
    {
        private ObservableCollection<Container> _containers;
        private DateTime _fromDate;
        private bool _isPurchaseSelected;
        private ObservableCollection<Purchase> _purchases;
        private ObservableCollection<Sale> _sales;
        private Container _selectedContainer;
        private Purchase _selectedPurchase;
        private Sale _selectedSale;
        private DateTime _toDate;

        public ReprintInvoiceViewModel()
        {
            this.Suppliers = new ReadOnlyObservableCollection<Supplier>(this.GetSuppliers());
            this.Products = new ReadOnlyObservableCollection<Product>(this.GetProducts());
            this.Customers = new ReadOnlyObservableCollection<Customer>(this.GetCustomers());
            this.PrintPurchaseCommand = new DelegateCommand(new Action(this.SomeShitGoingOn));
            this.PrintSaleCommand = new DelegateCommand(new Action(this.PrintSale));
            this.PrintContainerCommand = new DelegateCommand(new Action(this.PrintContainer));
            this.SearchCommand = new DelegateCommand(new Action(this.Search));
            this.GetWeighBridgeInfo();
            this.FromDate = DateTime.Now.AddDays(-1.0);
            this.ToDate = DateTime.Now;
            this.IsPurchaseSelected = true;
        }

        public Purchase SelectedPurchase
        {
            get
            {
                return this._selectedPurchase;
            }
            set
            {
                if (this._selectedPurchase == value)
                    return;
                this._selectedPurchase = value;
                this.OnPropertyChanged(nameof(SelectedPurchase));
            }
        }

        public Container SelectedContainer
        {
            get
            {
                return this._selectedContainer;
            }
            set
            {
                if (this._selectedContainer == value)
                    return;
                this._selectedContainer = value;
                this.OnPropertyChanged(nameof(SelectedContainer));
            }
        }

        public Sale SelectedSale
        {
            get
            {
                return _selectedSale;
            }
            set
            {
                if (_selectedSale == value)
                    return;
                this._selectedSale = value;
                this.OnPropertyChanged(nameof(SelectedSale));
            }
        }

        public DateTime FromDate
        {
            get
            {
                return this._fromDate;
            }
            set
            {
                if (!(this._fromDate != value))
                    return;
                this._fromDate = value;
                this.OnPropertyChanged(nameof(FromDate));
            }
        }

        public DateTime ToDate
        {
            get
            {
                return this._toDate;
            }
            set
            {
                if (!(this._toDate != value))
                    return;
                this._toDate = value;
                this.OnPropertyChanged(nameof(ToDate));
            }
        }

        public ObservableCollection<Purchase> Purchases
        {
            get
            {
                return this._purchases;
            }
            set
            {
                if (this._purchases == value)
                    return;
                this._purchases = value;
                this.OnPropertyChanged(nameof(Purchases));
            }
        }

        public ObservableCollection<Sale> Sales
        {
            get
            {
                return this._sales;
            }
            set
            {
                if (this._sales == value)
                    return;
                this._sales = value;
                this.OnPropertyChanged(nameof(Sales));
            }
        }

        public ObservableCollection<Container> Containers
        {
            get
            {
                return this._containers;
            }
            set
            {
                if (this._containers == value)
                    return;
                this._containers = value;
                this.OnPropertyChanged(nameof(Containers));
            }
        }

        public DelegateCommand PrintPurchaseCommand { get; set; }

        public DelegateCommand PrintContainerCommand { get; set; }

        public DelegateCommand PrintSaleCommand { get; set; }

        public DelegateCommand SearchCommand { get; set; }

        public bool IsPurchaseSelected
        {
            get
            {
                return this._isPurchaseSelected;
            }
            set
            {
                if (this._isPurchaseSelected == value)
                    return;
                this._isPurchaseSelected = value;
                this.OnPropertyChanged(nameof(IsPurchaseSelected));
            }
        }

        private void PrintContainer()
        {
            this.PrintContainerReceipt(this.SelectedContainer, "SALE", this.Customers.FirstOrDefault<Customer>((Func<Customer, bool>)(x => x.Bookings.Any<Booking>(d => d.ReferenceNumber == this.SelectedContainer.Booking.ReferenceNumber))));
        }

        private void PrintSale()
        {
            this.PrintSaleReceipt(this.SelectedSale, "SALE");
        }

        private void Search()
        {
            using (DataClassService dataClassService = new DataClassService())
            {
                IEnumerable<Purchase> purchases = dataClassService.SearchPurchase(this.FromDate, this.ToDate);
                this.Purchases = new ObservableCollection<Purchase>();
                foreach (Purchase purchase in purchases)
                    this.Purchases.Add(purchase);
            }
        }

        private void SomeShitGoingOn()
        {
            this.ViewBaseSupplier = this.Suppliers.FirstOrDefault<Supplier>((Func<Supplier, bool>)(s => s.Drivers.Any<Driver>((Func<Driver, bool>)(d => d.Id == this.SelectedPurchase.Driver.Id))));
            this.ViewBaseProduct = this.Products.FirstOrDefault<Product>((Func<Product, bool>)(p => p.Name == this.SelectedPurchase.WeighBridgeInfo.Product));
            this.PrintWeightBridgeReceipt(this.SelectedPurchase, this.ViewBaseSupplier, string.Empty);
        }

        private void GetWeighBridgeInfo()
        {
            using (DataClassService dataClassService = new DataClassService())
            {
                IEnumerable<Purchase> purchases = dataClassService.GetPurchases();
                this.Purchases = new ObservableCollection<Purchase>();
                foreach (Purchase purchase in (IEnumerable<Purchase>)purchases.OrderByDescending<Purchase, int>((Func<Purchase, int>)(w => w.WeighBridgeInfo.Id)).ToArray<Purchase>())
                    this.Purchases.Add(purchase);
            }
            this.GetLocalSales();
            this.LoadCompletedContainers();
        }

        private void LoadCompletedContainers()
        {
            using (DataClassService dataClassService = new DataClassService())
            {
                this.Containers = new ObservableCollection<Container>();
                foreach (Container completeContainer in dataClassService.GetCompleteContainers())
                    this.Containers.Add(completeContainer);
            }
        }

        private void GetLocalSales()
        {
            using (DataClassService dataClassService = new DataClassService())
            {
                IEnumerable<Sale> localSales = dataClassService.GetLocalSales();
                this.Sales = new ObservableCollection<Sale>();
                foreach (Sale sale in (IEnumerable<Sale>)localSales.OrderByDescending<Sale, int>((Func<Sale, int>)(w => w.WeighBridgeInfo.Id)).ToArray<Sale>())
                {
                    if (sale.Status != "FirstWeight")
                        this.Sales.Add(sale);
                }
            }
        }
    }
}
