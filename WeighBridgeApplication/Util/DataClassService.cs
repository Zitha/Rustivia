// Decompiled with JetBrains decompiler
// Type: WeighBridgeApplication.Util.DataClassService
// Assembly: WeighBridgeApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0008ECA4-ECDC-4057-A6F3-F7FD4CE2F23F
// Assembly location: C:\Users\Ndavhe\Desktop\Rustivia Weight Bridge\WeighBridgeApplication.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeighBridgeApplication.Model;
using WeighBridgeApplication.OfflineMode;
using WeighBridgeApplication.RustiviaService.DbContext;

namespace WeighBridgeApplication.Util
{
    public class DataClassService : IDisposable
    {
        private readonly DataServiceClient _client;

        public DataClassService()
        {
            this._client = new DataServiceClient();
        }

        public void Dispose()
        {
            if (this._client == null)
                return;
            this._client.Close();
        }

        public void AddContainerWeight(Container container)
        {
            ContainerDto containerDto = new ContainerDto()
            {
                ContainerNumber = container.ContainerNumber,
                Sealnumber = container.Sealnumber,
                Product = container.Product,
                TruckRegNumber = container.TruckRegNumber,
                WeighBridgeInfo = this.GetWeighBridgeInfoDto(container.WeighBridgeInfo),
                Status = container.Status
            };
            using (DataServiceClient _client = new DataServiceClient())
            {
                _client.AddContainerWeight(containerDto);
            }
        }

        public void AddSale(Sale sale)
        {
            SaleDto saleDto = new SaleDto()
            {
                ExtraInfo = sale.ExtraInfo,
                TruckRegNumber = sale.TruckRegNumber,
                WeighBridgeInfo = this.GetWeighBridgeInfoDto(sale.WeighBridgeInfo),
                Status = sale.Status,
                Customer = new CustomerDto()
                {
                    Id = sale.Customer.Id
                }
            };
            Task.Factory.StartNew<Task>((Func<Task>)(() => this._client.AddSaleAsync(saleDto)));
        }

        public void AddPurchase(Purchase purchase)
        {
            PurchaseDto purchaseDto = new PurchaseDto()
            {
                Driver = this.GetDriverDto(purchase.Driver),
                Truck = this.GetTruckDto(purchase.Truck),
                WeighBridgeInfo = this.GetWeighBridgeInfoDto(purchase.WeighBridgeInfo),
                Status = purchase.Status
            };
            using (DataServiceClient _client = new DataServiceClient())
            {
                _client.AddPurchase(purchaseDto);
            }
        }

        private DriverDto GetDriverDto(Driver driver)
        {
            return new DriverDto()
            {
                Id = driver.Id,
                Firstname = driver.Firstname,
                Surname = driver.Surname,
                IdNumber = driver.IdNumber
            };
        }

        private TruckDto GetTruckDto(Truck truck)
        {
            TruckDto truckDto = new TruckDto();
            truckDto.Id = truck.Id;
            truckDto.TruckRegNumber = truck.TruckRegNumber;
            int num = truck.Own ? 1 : 0;
            truckDto.Own = num != 0;
            return truckDto;
        }

        public void UpdateSale(Sale sale)
        {
            SaleDto saleDto1 = new SaleDto();
            saleDto1.ExtraInfo = sale.ExtraInfo;
            saleDto1.TruckRegNumber = sale.TruckRegNumber;
            saleDto1.WeighBridgeInfo = this.GetWeighBridgeInfoDto(sale.WeighBridgeInfo);
            saleDto1.Status = sale.Status;
            saleDto1.Customer = new CustomerDto()
            {
                Id = sale.Customer.Id
            };
            int id = sale.Id;
            saleDto1.Id = id;
            SaleDto saleDto = saleDto1;
            Task.Factory.StartNew<Task>((Func<Task>)(() => this._client.UpdateSaleAsync(saleDto)));
        }

        public void UpdatePurchase(Purchase purchase)
        {
            PurchaseDto purchaseDto = new PurchaseDto()
            {
                WeighBridgeInfo = this.GetWeighBridgeInfoDto(purchase.WeighBridgeInfo),
                Status = purchase.Status,
                Price = purchase.Price,
                TotalPrice = purchase.TotalPrice
            };
            Task.Factory.StartNew<Task>((Func<Task>)(() => this._client.UpdatePurchaseAsync(purchaseDto)));
        }

        public void UpdateContainer(Container container, long nettWight)
        {
            ContainerDto containerDto = new ContainerDto()
            {
                ContainerNumber = container.ContainerNumber,
                Sealnumber = container.Sealnumber,
                Product = container.Product,
                TruckRegNumber = container.TruckRegNumber,
                WeighBridgeInfo = this.GetWeighBridgeInfoDto(container.WeighBridgeInfo),
                Status = container.Status,
                NettWeight = nettWight
            };
            Task task = (Task)Task.Factory.StartNew<Task>((Func<Task>)(() => this._client.UpdateContainerAsync(containerDto)));
        }

        public IEnumerable<Customer> GetCustomers()
        {
            List<Customer> customers = ((IEnumerable<CustomerDto>)Task.Factory.StartNew<CustomerDto[]>((Func<CustomerDto[]>)(() => this._client.GetCustomersAsync().Result)).Result).Select<CustomerDto, Customer>((Func<CustomerDto, Customer>)(customer => new Customer()
            {
                Id = customer.Id,
                CustomerCode = customer.CustomerCode,
                CustomerName = customer.CustomerName,
                Address = customer.Address,
                Bookings = this.GetBookings((IEnumerable<BookingDto>)customer.Bookings)
            })).ToList<Customer>();
            DataClassJson dataClass = new DataClassJson();
            try
            {
                Task.Factory.StartNew((Action)(() => dataClass.CacheCustomers(customers.ToArray())));
            }
            finally
            {
                if (dataClass != null)
                    dataClass.Dispose();
            }
            return (IEnumerable<Customer>)customers;
        }

        public IEnumerable<Supplier> GetSuppliers()
        {
            Task<SupplierInfoDto[]> task = Task.Factory.StartNew<SupplierInfoDto[]>((Func<SupplierInfoDto[]>)(() => this._client.GetSuppliersAsync().Result));
            task.Wait();
            List<Supplier> list = ((IEnumerable<SupplierInfoDto>)task.Result).Select<SupplierInfoDto, Supplier>((Func<SupplierInfoDto, Supplier>)(supplier => new Supplier()
            {
                Id = supplier.Id,
                SupplierName = supplier.SupplierName,
                Suppliercode = supplier.SupplierCode,
                Drivers = this.GetDrivers((IEnumerable<DriverDto>)supplier.Drivers),
                Trucks = this.GetSupplierTrucks((IEnumerable<TruckDto>)supplier.Trucks),
                SupplierProducts = this.GetSupplierProduct((IEnumerable<SupplierProductDto>)supplier.SupplierProductDtos)
            })).ToList<Supplier>();
            using (DataClassJson dataClassJson = new DataClassJson())
                dataClassJson.CacheSuppliers(list.ToArray());
            return (IEnumerable<Supplier>)list;
        }

        public IEnumerable<Product> GetProducts()
        {
            List<Product> products = ((IEnumerable<ProductDto>)Task.Factory.StartNew<ProductDto[]>((Func<ProductDto[]>)(() => this._client.GetProductsAsync().Result)).Result).Select<ProductDto, Product>((Func<ProductDto, Product>)(product => new Product()
            {
                Id = product.Id,
                Name = product.Name,
                RustiviaPrice = product.RustiviaPrice
            })).ToList<Product>();
            DataClassJson dataClass = new DataClassJson();
            try
            {
                Task.Factory.StartNew((Action)(() => dataClass.CacheProducts(products.ToArray())));
            }
            finally
            {
                if (dataClass != null)
                    dataClass.Dispose();
            }
            return (IEnumerable<Product>)products;
        }

        public IEnumerable<Truck> GetTrucks()
        {
            List<Truck> trucks = ((IEnumerable<TruckDto>)Task.Factory.StartNew<TruckDto[]>((Func<TruckDto[]>)(() => this._client.GetTrucksAsync().Result)).Result).Select<TruckDto, Truck>((Func<TruckDto, Truck>)(truck =>
            {
                Truck truck1 = new Truck();
                truck1.Id = truck.Id;
                truck1.TruckRegNumber = truck.TruckRegNumber;
                int num = truck.Own ? 1 : 0;
                truck1.Own = num != 0;
                return truck1;
            })).ToList<Truck>();
            DataClassJson dataClass = new DataClassJson();
            try
            {
                Task.Factory.StartNew((Action)(() => dataClass.CacheTrucks(trucks.ToArray())));
            }
            finally
            {
                if (dataClass != null)
                    dataClass.Dispose();
            }
            return (IEnumerable<Truck>)trucks;
        }

        public IEnumerable<Driver> GetDrivers()
        {
            List<Driver> drivers = ((IEnumerable<DriverDto>)Task.Factory.StartNew<DriverDto[]>((Func<DriverDto[]>)(() => this._client.GetDriversAsync().Result)).Result).Select<DriverDto, Driver>((Func<DriverDto, Driver>)(driver => new Driver()
            {
                Id = driver.Id,
                Firstname = driver.Firstname,
                Surname = driver.Surname,
                FullName = string.Format("{0} {1}", (object)driver.Firstname, (object)driver.Surname),
                IdNumber = driver.IdNumber,
                IdLocation = driver.IdNumber
            })).ToList<Driver>();
            DataClassJson dataClass = new DataClassJson();
            try
            {
                Task.Factory.StartNew((Action)(() => dataClass.CacheDrivers(drivers.ToArray())));
            }
            finally
            {
                if (dataClass != null)
                    dataClass.Dispose();
            }
            return (IEnumerable<Driver>)drivers;
        }

        private List<Booking> GetBookings(IEnumerable<BookingDto> bookings)
        {
            return bookings.Select<BookingDto, Booking>((Func<BookingDto, Booking>)(booking => new Booking()
            {
                ReferenceNumber = booking.ReferenceNumber,
                Containers = this.GetContainers(booking.Containers)
            })).ToList<Booking>();
        }

        public bool Logon(string username, string password)
        {
            bool result;
            try
            {
                Task<bool> task = Task.Factory.StartNew<bool>((Func<bool>)(() => this._client.IsValidUser(username, password)));
                task.Wait();
                result = task.Result;
            }
            catch (Exception ex)
            {
                EventLogHelper.Log(ex.Message);
                throw;
            }
            return result;
        }

        internal void RecoverPurchase(Purchase purchase)
        {
            PurchaseDto purchaseDto = new PurchaseDto()
            {
                Driver = this.GetDriverDto(purchase.Driver),
                Truck = this.GetTruckDto(purchase.Truck),
                WeighBridgeInfo = this.GetWeighBridgeInfoDto(purchase.WeighBridgeInfo),
                Status = purchase.Status,
                Id = purchase.Id
            };
            Task task = (Task)Task.Factory.StartNew<Task>((Func<Task>)(() => this._client.RecoverPurchaseAsync(purchaseDto)));
        }

        internal void RecoverSale(Sale sale)
        {
            SaleDto saleDto1 = new SaleDto();
            saleDto1.ExtraInfo = sale.ExtraInfo;
            saleDto1.TruckRegNumber = sale.TruckRegNumber;
            saleDto1.WeighBridgeInfo = this.GetWeighBridgeInfoDto(sale.WeighBridgeInfo);
            saleDto1.Status = sale.Status;
            saleDto1.Customer = new CustomerDto()
            {
                Id = sale.Customer.Id
            };
            int id = sale.Id;
            saleDto1.Id = id;
            SaleDto saleDto = saleDto1;
            Task.Factory.StartNew<Task>((Func<Task>)(() => this._client.RecoverSaleAsync(saleDto)));
        }

        public IEnumerable<Purchase> GetIncompletePurchases()
        {
            using (DataServiceClient _client = new DataServiceClient())
            {
                var results = _client.GetIncompletePurchase();

                return results.Select(incompletePurchase => new Purchase()
                {
                    Id = incompletePurchase.Id,
                    Status = incompletePurchase.Status,
                    WeighBridgeInfo = this.GetWeighBridgeInfo(incompletePurchase.WeighBridgeInfo),
                    Driver = this.GetDriver(incompletePurchase.Driver),
                    Truck = this.GetTruck(incompletePurchase.Truck)
                }).ToList();
            }
            //return ((IEnumerable<PurchaseDto>)this._client.GetIncompletePurchaseAsync().Result).Select(incompletePurchase => new Purchase()
            //{
            //    Id = incompletePurchase.Id,
            //    Status = incompletePurchase.Status,
            //    WeighBridgeInfo = this.GetWeighBridgeInfo(incompletePurchase.WeighBridgeInfo),
            //    Driver = this.GetDriver(incompletePurchase.Driver),
            //    Truck = this.GetTruck(incompletePurchase.Truck)
            //}).ToList<Purchase>();
        }

        public IEnumerable<Sale> GetIncompleteSales()
        {
            return (IEnumerable<Sale>)((IEnumerable<SaleDto>)this._client.GetIncompleteSalesAsync().Result).Select<SaleDto, Sale>((Func<SaleDto, Sale>)(incompleteSale => new Sale()
            {
                Id = incompleteSale.Id,
                Status = incompleteSale.Status,
                TruckRegNumber = incompleteSale.TruckRegNumber,
                ExtraInfo = incompleteSale.ExtraInfo,
                Customer = this.GetCustomer(incompleteSale.Customer),
                WeighBridgeInfo = this.GetWeighBridgeInfo(incompleteSale.WeighBridgeInfo)
            })).ToList<Sale>();
        }

        public IEnumerable<Sale> GetLocalSales()
        {
            return (IEnumerable<Sale>)((IEnumerable<SaleDto>)this._client.GetSalesAsync().Result).Select<SaleDto, Sale>((Func<SaleDto, Sale>)(localSale => new Sale()
            {
                Id = localSale.Id,
                Status = localSale.Status,
                TruckRegNumber = localSale.TruckRegNumber,
                ExtraInfo = localSale.ExtraInfo,
                Customer = this.GetCustomer(localSale.Customer),
                WeighBridgeInfo = this.GetWeighBridgeInfo(localSale.WeighBridgeInfo)
            })).ToList<Sale>();
        }

        public IEnumerable<Container> GetIncompleteContainers()
        {
            return (IEnumerable<Container>)((IEnumerable<ContainerDto>)this._client.GetIncompleteContainersAsync().Result).Select<ContainerDto, Container>((Func<ContainerDto, Container>)(container => new Container()
            {
                ContainerNumber = container.ContainerNumber,
                Sealnumber = container.Sealnumber,
                TruckRegNumber = container.TruckRegNumber,
                TareWeight = container.TareWeight,
                Product = container.Product,
                WeighBridgeInfo = this.GetWeighBridgeInfo(container.WeighBridgeInfo),
                Booking = this.GetBooking(container.Booking)
            })).ToList<Container>();
        }

        public IEnumerable<Purchase> SearchPurchase(DateTime fromDate, DateTime toDate)
        {
            return (IEnumerable<Purchase>)((IEnumerable<PurchaseDto>)((IEnumerable<PurchaseDto>)this._client.SearchPurchaseAsync(fromDate, toDate).Result).OrderByDescending<PurchaseDto, int>((Func<PurchaseDto, int>)(w => w.WeighBridgeInfo.Id)).ToArray<PurchaseDto>()).Where<PurchaseDto>((Func<PurchaseDto, bool>)(purchaseDto => purchaseDto.Status != "FirstWeight")).Select(purchaseDto =>
            {
                PurchaseDto purchaseDto1 = purchaseDto;
                return new
                {
                    purchaseDto = purchaseDto1,
                    weighBridge = new WeighBridgeInfo()
                    {
                        Id = purchaseDto.WeighBridgeInfo.Id,
                        FirstMass = purchaseDto.WeighBridgeInfo.FirstMass,
                        SecondMass = purchaseDto.WeighBridgeInfo.SecondMass,
                        NettMass = purchaseDto.WeighBridgeInfo.NettMass,
                        DateIn = purchaseDto.WeighBridgeInfo.DateIn,
                        DateOut = purchaseDto.WeighBridgeInfo.DateOut.Value,
                        Product = purchaseDto.WeighBridgeInfo.Product,
                        Comments = purchaseDto.WeighBridgeInfo.Comments
                    }
                };
            }).Select(_param1 => new Purchase()
            {
                Driver = this.GetDriver(_param1.purchaseDto.Driver),
                Truck = this.GetTruck(_param1.purchaseDto.Truck),
                Id = _param1.purchaseDto.Id,
                Status = _param1.purchaseDto.Status,
                WeighBridgeInfo = _param1.weighBridge
            }).ToList<Purchase>();
        }

        public IEnumerable<Purchase> GetPurchases()
        {
            return (IEnumerable<Purchase>)((IEnumerable<PurchaseDto>)((IEnumerable<PurchaseDto>)this._client.GetPurchaseAsync().Result).OrderByDescending<PurchaseDto, int>((Func<PurchaseDto, int>)(w => w.WeighBridgeInfo.Id)).ToArray<PurchaseDto>()).Where<PurchaseDto>((Func<PurchaseDto, bool>)(purchaseDto => purchaseDto.Status != "FirstWeight")).Select(purchaseDto =>
            {
                PurchaseDto purchaseDto1 = purchaseDto;
                return new
                {
                    purchaseDto = purchaseDto1,
                    weighBridge = new WeighBridgeInfo()
                    {
                        Id = purchaseDto.WeighBridgeInfo.Id,
                        FirstMass = purchaseDto.WeighBridgeInfo.FirstMass,
                        SecondMass = purchaseDto.WeighBridgeInfo.SecondMass,
                        NettMass = purchaseDto.WeighBridgeInfo.NettMass,
                        DateIn = purchaseDto.WeighBridgeInfo.DateIn,
                        DateOut = purchaseDto.WeighBridgeInfo.DateOut.HasValue ? purchaseDto.WeighBridgeInfo.DateOut.Value : DateTime.Now.Date,
                        Product = purchaseDto.WeighBridgeInfo.Product,
                        Comments = purchaseDto.WeighBridgeInfo.Comments
                    }
                };
            }).Select(_param1 => new Purchase()
            {
                Driver = this.GetDriver(_param1.purchaseDto.Driver),
                Truck = this.GetTruck(_param1.purchaseDto.Truck),
                Id = _param1.purchaseDto.Id,
                Status = _param1.purchaseDto.Status,
                WeighBridgeInfo = _param1.weighBridge
            }).ToList<Purchase>();
        }

        public IEnumerable<Container> GetCompleteContainers()
        {
            return (IEnumerable<Container>)((IEnumerable<ContainerDto>)this._client.GetIncompleteContainersAsync().Result).Select<ContainerDto, Container>((Func<ContainerDto, Container>)(container => new Container()
            {
                ContainerNumber = container.ContainerNumber,
                Sealnumber = container.Sealnumber,
                TruckRegNumber = container.TruckRegNumber,
                TareWeight = container.TareWeight,
                Product = container.Product,
                WeighBridgeInfo = this.GetWeighBridgeInfo(container.WeighBridgeInfo),
                Booking = this.GetBooking(container.Booking)
            })).ToList<Container>();
        }

        private WeighBridgeInfo GetWeighBridgeInfo(WeighBridgeInfoDto weighBridgeInfo)
        {
            return new WeighBridgeInfo()
            {
                Id = weighBridgeInfo.Id,
                Comments = weighBridgeInfo.Comments,
                DateIn = weighBridgeInfo.DateIn,
                FirstMass = weighBridgeInfo.FirstMass,
                NettMass = weighBridgeInfo.NettMass,
                Product = weighBridgeInfo.Product,
                SecondMass = weighBridgeInfo.SecondMass
            };
        }

        private Customer GetCustomer(CustomerDto customerDto)
        {
            return new Customer() { Id = customerDto.Id };
        }

        private Truck GetTruck(TruckDto truckDto)
        {
            Truck truck = new Truck();
            truck.Id = truckDto.Id;
            truck.TruckRegNumber = truckDto.TruckRegNumber;
            int num = truckDto.Own ? 1 : 0;
            truck.Own = num != 0;
            return truck;
        }

        private Driver GetDriver(DriverDto driver)
        {
            return new Driver()
            {
                Id = driver.Id,
                Firstname = driver.Firstname,
                Surname = driver.Surname,
                IdNumber = driver.IdNumber
            };
        }

        private Booking GetBooking(BookingDto booking)
        {
            return new Booking()
            {
                Containers = this.GetContainers((IEnumerable<ContainerDto>)booking.Containers),
                ReferenceNumber = booking.ReferenceNumber
            };
        }

        private List<Container> GetContainers(IEnumerable<ContainerDto> containers)
        {
            return containers.Select<ContainerDto, Container>((Func<ContainerDto, Container>)(a => new Container()
            {
                ContainerNumber = a.ContainerNumber,
                Sealnumber = a.Sealnumber,
                Product = a.Product
            })).ToList<Container>();
        }

        private WeighBridgeInfoDto GetWeighBridgeInfoDto(WeighBridgeInfo weighBridgeInfo)
        {
            return new WeighBridgeInfoDto()
            {
                Id = weighBridgeInfo.Id,
                Comments = weighBridgeInfo.Comments,
                DateIn = weighBridgeInfo.DateIn,
                FirstMass = weighBridgeInfo.FirstMass,
                NettMass = weighBridgeInfo.NettMass,
                Product = weighBridgeInfo.Product,
                SecondMass = weighBridgeInfo.SecondMass,
                DateOut = new DateTime?(weighBridgeInfo.DateOut)
            };
        }

        private List<Driver> GetDrivers(IEnumerable<DriverDto> drivers)
        {
            return drivers.Select<DriverDto, Driver>((Func<DriverDto, Driver>)(driver => new Driver()
            {
                Id = driver.Id,
                Firstname = driver.Firstname,
                Surname = driver.Surname,
                IdNumber = driver.IdNumber,
                FullName = string.Format("{0} {1}", (object)driver.Surname, (object)driver.Firstname),
                SupplierInfo = (Supplier)null
            })).ToList<Driver>();
        }

        private List<Truck> GetSupplierTrucks(IEnumerable<TruckDto> trucks)
        {
            return trucks.Select<TruckDto, Truck>((Func<TruckDto, Truck>)(truck =>
            {
                Truck truck1 = new Truck();
                truck1.Id = truck.Id;
                truck1.TruckRegNumber = truck.TruckRegNumber;
                int num = truck.Own ? 1 : 0;
                truck1.Own = num != 0;
                return truck1;
            })).ToList<Truck>();
        }

        private List<SupplierProduct> GetSupplierProduct(IEnumerable<SupplierProductDto> supplierProductDto)
        {
            return supplierProductDto.Select<SupplierProductDto, SupplierProduct>((Func<SupplierProductDto, SupplierProduct>)(sp => new SupplierProduct()
            {
                ProductId = sp.ProductId,
                SupplierId = sp.SupplierId,
                SupplierPrice = sp.SupplierPrice
            })).ToList<SupplierProduct>();
        }

        private Supplier GetSupplier(SupplierInfoDto supplier)
        {
            return new Supplier()
            {
                Id = supplier.Id,
                SupplierName = supplier.SupplierName,
                Drivers = this.GetDrivers((IEnumerable<DriverDto>)supplier.Drivers),
                Trucks = this.GetSupplierTrucks((IEnumerable<TruckDto>)supplier.Trucks),
                Suppliercode = supplier.SupplierCode
            };
        }
    }
}
