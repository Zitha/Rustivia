// Decompiled with JetBrains decompiler
// Type: WeighBridgeApplication.ViewModel.DeliveryViewModel
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
    public class DeliveryViewModel : ViewModelBase
    {
        private string _address;
        private string _comments;
        private ObservableCollection<Container> _containerList;
        private string _containerNumber;
        private ReadOnlyObservableCollection<Customer> _customerList;
        private string _extraInfo;
        private bool _isLocal;
        private bool _isValid;
        private string _productType;
        private ObservableCollection<Booking> _referenceNumberList;
        private string _sealNumber;
        private Container _selectedContainer;
        private Customer _selectedCustomer;
        private Booking _selectedReferenceNumber;
        private string _truckRegNum;
        private ReadOnlyObservableCollection<Product> _productList;
        private ReadOnlyObservableCollection<Product> _truckList;
        private Product _selectedProduct;

        public DeliveryViewModel(Container container = null, Sale sale = null)
        {
            this.ProductList = new ReadOnlyObservableCollection<Product>(this.GetProducts());
            this.ReferenceNumberList = new ObservableCollection<Booking>();
            this.CustomerList = new ReadOnlyObservableCollection<Customer>(this.GetCustomers());
            if (container != null)
            {
                foreach (Customer customer in (ReadOnlyCollection<Customer>)this.CustomerList)
                {
                    foreach (Booking booking in customer.Bookings)
                    {
                        if (booking.Containers.FirstOrDefault<Container>((Func<Container, bool>)(a => a.ContainerNumber == container.ContainerNumber)) != null)
                        {
                            this.SelectedCustomer = customer;
                            this.SelectedReferenceNumber = booking;
                            break;
                        }
                    }
                }
                this.SelectedProduct = this.ProductList.FirstOrDefault<Product>((Func<Product, bool>)(pr => pr.Name == container.WeighBridgeInfo.Product));
                this.SelectedContainer = container;
                this.TruckRegNum = container.TruckRegNumber;
            }
            if (sale == null)
                return;
            foreach (Customer customer in (ReadOnlyCollection<Customer>)this.CustomerList)
            {
                if (customer.Id == sale.Customer.Id)
                {
                    this.SelectedCustomer = customer;
                    break;
                }
            }
            this.TruckRegNum = sale.TruckRegNumber;
            this.ProductType = sale.WeighBridgeInfo.Product;
            this.ExtraInfo = sale.ExtraInfo;
            this.SelectedProduct = this.ProductList.FirstOrDefault<Product>((Func<Product, bool>)(pr => pr.Name == sale.WeighBridgeInfo.Product));
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

        public ReadOnlyObservableCollection<Customer> CustomerList
        {
            get
            {
                return this._customerList;
            }
            set
            {
                if (this._customerList == value)
                    return;
                this._customerList = value;
                this.OnPropertyChanged(nameof(CustomerList));
            }
        }

        public ObservableCollection<Booking> ReferenceNumberList
        {
            get
            {
                return this._referenceNumberList;
            }
            set
            {
                if (this._referenceNumberList == value)
                    return;
                this._referenceNumberList = value;
                this.OnPropertyChanged(nameof(ReferenceNumberList));
            }
        }

        public ObservableCollection<Container> ContainerList
        {
            get
            {
                return this._containerList;
            }
            set
            {
                if (this._containerList == value)
                    return;
                this._containerList = value;
                this.OnPropertyChanged(nameof(ContainerList));
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

        public Booking SelectedReferenceNumber
        {
            get
            {
                return this._selectedReferenceNumber;
            }
            set
            {
                if (this._selectedReferenceNumber == value)
                    return;
                this._selectedReferenceNumber = value;
                if (this._selectedReferenceNumber != null)
                {
                    this.ContainerList = new ObservableCollection<Container>();
                    this._selectedReferenceNumber.Containers.ForEach(a => this.ContainerList.Add(new Container()
                    {
                        ContainerNumber = a.ContainerNumber,
                        Sealnumber = a.Sealnumber,
                        Product = a.Product
                    }));
                }
                this.OnPropertyChanged(nameof(SelectedReferenceNumber));
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
                if (_selectedContainer!=null)
                {
                    this.SealNumber = this._selectedContainer.Sealnumber;
                    this.ProductType = this._selectedContainer.Product;
                    this.ContainerNumber = this._selectedContainer.ContainerNumber;
                }
            }
        }

        public string ContainerNumber
        {
            get
            {
                return this._containerNumber;
            }
            set
            {
                if (!(this._containerNumber != value))
                    return;
                this._containerNumber = value;
                this.OnPropertyChanged(nameof(ContainerNumber));
            }
        }

        public string SealNumber
        {
            get
            {
                return this._sealNumber;
            }
            set
            {
                if (!(this._sealNumber != value))
                    return;
                this._sealNumber = value;
                this.OnPropertyChanged(nameof(SealNumber));
            }
        }

        public string TruckRegNum
        {
            get
            {
                return this._truckRegNum;
            }
            set
            {
                if (!(this._truckRegNum != value))
                    return;
                this._truckRegNum = value;
                this.OnPropertyChanged(nameof(TruckRegNum));
            }
        }

        public Customer SelectedCustomer
        {
            get
            {
                return this._selectedCustomer;
            }
            set
            {
                if (this._selectedCustomer == value)
                    return;
                this._selectedCustomer = value;
                if (this._selectedCustomer != null)
                {
                    this.ReferenceNumberList = new ObservableCollection<Booking>();
                    foreach (Booking booking in this._selectedCustomer.Bookings)
                    {
                        if (booking.Containers.Count > 0)
                            this.ReferenceNumberList.Add(booking);
                    }
                    this.Address = this._selectedCustomer.Address;
                    ContainerList = new ObservableCollection<Container>();
                }
                this.IsModelValid();
                this.OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        public string ProductType
        {
            get
            {
                return this._productType;
            }
            set
            {
                if (!(this._productType != value))
                    return;
                this._productType = value;
                this.OnPropertyChanged(nameof(ProductType));
            }
        }

        public string Address
        {
            get
            {
                return this._address;
            }
            set
            {
                if (!(this._address != value))
                    return;
                this._address = value;
                this.OnPropertyChanged(nameof(Address));
            }
        }

        public string ExtraInfo
        {
            get
            {
                return this._extraInfo;
            }
            set
            {
                if (!(this._extraInfo != value))
                    return;
                this._extraInfo = value;
                this.OnPropertyChanged(nameof(ExtraInfo));
            }
        }

        public bool IsLocal
        {
            get
            {
                return this._isLocal;
            }
            set
            {
                if (this._isLocal == value)
                    return;
                this._isLocal = value;
                this.ReferenceNumberList = new ObservableCollection<Booking>();
                this.ContainerList = new ObservableCollection<Container>();
                this.OnPropertyChanged(nameof(IsLocal));
            }
        }

        public event EventHandler IsValidEvent;

        private void IsModelValid()
        {
            this.IsValid = this.SelectedCustomer != null;
            // ISSUE: reference to a compiler-generated field
            if (this.IsValidEvent == null)
                return;
            // ISSUE: reference to a compiler-generated field
            this.IsValidEvent((object)this, new EventArgs());
        }
    }
}
