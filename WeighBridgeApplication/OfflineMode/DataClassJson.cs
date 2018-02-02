// Decompiled with JetBrains decompiler
// Type: WeighBridgeApplication.OfflineMode.DataClassJson
// Assembly: WeighBridgeApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0008ECA4-ECDC-4057-A6F3-F7FD4CE2F23F
// Assembly location: C:\Users\Ndavhe\Desktop\Rustivia Weight Bridge\WeighBridgeApplication.exe

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using WeighBridgeApplication.Model;

namespace WeighBridgeApplication.OfflineMode
{
    public class DataClassJson : IDisposable
    {
        private readonly string _usersData = Path.Combine(Environment.CurrentDirectory, "DataSource", "Users.json");
        private readonly string _driverData = Path.Combine(Environment.CurrentDirectory, "DataSource", "Drivers.json");
        private readonly string _supplierData = Path.Combine(Environment.CurrentDirectory, "DataSource", "Supplier.json");
        private readonly string _truckData = Path.Combine(Environment.CurrentDirectory, "DataSource", "Trucks.json");
        private readonly string _productData = Path.Combine(Environment.CurrentDirectory, "DataSource", "Product.json");
        private readonly string _customerData = Path.Combine(Environment.CurrentDirectory, "DataSource", "Customers.json");

        internal void CacheUser(string userName, string password)
        {
            if (!File.Exists(this._usersData))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "DataSource"));
                this.CreateFile(this._usersData);
            }
            List<User> userList;
            using (StreamReader streamReader = new StreamReader(this._usersData))
            {
                string end = streamReader.ReadToEnd();
                userList = !string.IsNullOrEmpty(end) ? (List<User>)JsonConvert.DeserializeObject(end, typeof(List<User>)) : new List<User>();
            }
            using (FileStream fileStream = File.Open(this._usersData, FileMode.Open))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream))
                {
                    using (JsonWriter jsonWriter = (JsonWriter)new JsonTextWriter((TextWriter)streamWriter))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        JsonSerializer jsonSerializer = new JsonSerializer();
                        userList.Add(new User()
                        {
                            Password = password,
                            UserName = userName
                        });
                        jsonSerializer.Serialize(jsonWriter, (object)userList);
                    }
                }
            }
        }

        internal bool Logon(string userName, string password)
        {
            List<User> userList;
            using (StreamReader streamReader = new StreamReader(this._usersData))
                userList = (List<User>)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(List<User>));
            foreach (User user in userList)
            {
                if (user.Password == password && user.UserName == userName)
                    return true;
            }
            return false;
        }

        public void CacheSuppliers(Supplier[] supplierInfo)
        {
            if (!File.Exists(this._supplierData))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "DataSource"));
                this.CreateFile(this._supplierData);
            }
            File.GetLastWriteTime(this._supplierData);
            using (FileStream fileStream = File.Open(this._supplierData, FileMode.Open))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream))
                {
                    using (JsonWriter jsonWriter = (JsonWriter)new JsonTextWriter((TextWriter)streamWriter))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        new JsonSerializer().Serialize(jsonWriter, (object)supplierInfo);
                    }
                }
            }
        }

        private void CreateFile(string filePath)
        {
            using (File.Create(Path.Combine(filePath)))
                ;
        }

        public List<Supplier> LoadSuppliers()
        {
            List<Supplier> supplierList;
            using (StreamReader streamReader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "DataSource", "Supplier.json")))
                supplierList = (List<Supplier>)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(List<Supplier>));
            return supplierList;
        }

        public void CacheDrivers(Driver[] driver)
        {
            if (!File.Exists(this._driverData))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "DataSource"));
                this.CreateFile(this._driverData);
            }
            File.GetLastWriteTime(this._driverData);
            using (FileStream fileStream = File.Open(this._driverData, FileMode.Open))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream))
                {
                    using (JsonWriter jsonWriter = (JsonWriter)new JsonTextWriter((TextWriter)streamWriter))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        new JsonSerializer().Serialize(jsonWriter, (object)driver);
                    }
                }
            }
        }

        public List<Driver> LoadDrivers()
        {
            List<Driver> driverList;
            using (StreamReader streamReader = new StreamReader(this._supplierData))
                driverList = (List<Driver>)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(List<Driver>));
            return driverList;
        }

        public void CacheTrucks(Truck[] truck)
        {
            if (!File.Exists(this._truckData))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "DataSource"));
                this.CreateFile(this._truckData);
            }
            File.GetLastWriteTime(this._truckData);
            using (FileStream fileStream = File.Open(this._truckData, FileMode.Open))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream))
                {
                    using (JsonWriter jsonWriter = (JsonWriter)new JsonTextWriter((TextWriter)streamWriter))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        new JsonSerializer().Serialize(jsonWriter, (object)truck);
                    }
                }
            }
        }

        public List<Truck> LoadTrucks()
        {
            List<Truck> truckList;
            using (StreamReader streamReader = new StreamReader(this._truckData))
                truckList = (List<Truck>)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(List<Truck>));
            return truckList;
        }

        public void Dispose()
        {
        }

        public void CacheProducts(Product[] products)
        {
            if (!File.Exists(this._productData))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "DataSource"));
                this.CreateFile(this._productData);
            }
            File.GetLastWriteTime(this._productData);
            using (FileStream fileStream = File.Open(this._productData, FileMode.Open))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream))
                {
                    using (JsonWriter jsonWriter = (JsonWriter)new JsonTextWriter((TextWriter)streamWriter))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        new JsonSerializer().Serialize(jsonWriter, (object)products);
                    }
                }
            }
        }

        internal IEnumerable<Product> LoadProducts()
        {
            List<Product> productList;
            using (StreamReader streamReader = new StreamReader(this._productData))
                productList = (List<Product>)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(List<Product>));
            return (IEnumerable<Product>)productList;
        }

        internal void CacheCustomers(Customer[] customers)
        {
            if (!File.Exists(this._customerData))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "DataSource"));
                this.CreateFile(this._customerData);
            }
            File.GetLastWriteTime(this._customerData);
            using (FileStream fileStream = File.Open(this._customerData, FileMode.Open))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream))
                {
                    using (JsonWriter jsonWriter = (JsonWriter)new JsonTextWriter((TextWriter)streamWriter))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        new JsonSerializer().Serialize(jsonWriter, (object)customers);
                    }
                }
            }
        }

        internal IEnumerable<Customer> LoadCustomers()
        {
            List<Customer> customerList;
            using (StreamReader streamReader = new StreamReader(this._customerData))
                customerList = (List<Customer>)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(List<Customer>));
            return (IEnumerable<Customer>)customerList;
        }
    }
}
