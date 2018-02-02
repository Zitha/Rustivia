// Decompiled with JetBrains decompiler
// Type: WeighBridgeApplication.OfflineMode.WeighModeJson
// Assembly: WeighBridgeApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0008ECA4-ECDC-4057-A6F3-F7FD4CE2F23F
// Assembly location: C:\Users\Ndavhe\Desktop\Rustivia Weight Bridge\WeighBridgeApplication.exe

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WeighBridgeApplication.Model;
using WeighBridgeApplication.Util;

namespace WeighBridgeApplication.OfflineMode
{
    internal class WeighModeJson : IDisposable
    {
        private readonly string _containersData = Path.Combine(Environment.CurrentDirectory, "DataSource", "Containers.json");
        private readonly string _purchaseData = Path.Combine(Environment.CurrentDirectory, "DataSource", "Purchase.json");
        private readonly string _salesData = Path.Combine(Environment.CurrentDirectory, "DataSource", "Sales.json");
        private readonly string _wbCounter = Path.Combine(Environment.CurrentDirectory, "DataSource", "WeighBridgeCounter.txt");

        public void Dispose()
        {
        }

        private void CreateFile(string filePath)
        {
            using (File.Create(Path.Combine(filePath)))
                ;
        }

        internal IEnumerable<Container> GetIncompleteContainers()
        {
            if (!File.Exists(this._containersData))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "DataSource"));
                this.CreateFile(this._containersData);
            }
            List<Container> containerList1 = new List<Container>();
            List<Container> containerList2;
            using (StreamReader streamReader = new StreamReader(this._containersData))
                containerList2 = (List<Container>)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(List<Container>));
            if (containerList2 == null)
                return new List<Container>();
            foreach (Container container in containerList2)
            {
                if (container.Status == "FirstWeight")
                    containerList1.Add(container);
            }
            return containerList1;
        }

        internal IEnumerable<Purchase> GetIncompletePurchases()
        {
            if (!File.Exists(this._purchaseData))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "DataSource"));
                this.CreateFile(this._purchaseData);
            }
            List<Purchase> purchaseList1 = new List<Purchase>();
            List<Purchase> purchaseList2;
            using (StreamReader streamReader = new StreamReader(this._purchaseData))
                purchaseList2 = (List<Purchase>)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(List<Purchase>));
            if (purchaseList2 == null)
                return new List<Purchase>();
            foreach (Purchase purchase in purchaseList2)
            {
                if (purchase.Status == "FirstWeight")
                    purchaseList1.Add(purchase);
            }
            return purchaseList1;
        }

        internal IEnumerable<Sale> GetIncompleteSales()
        {
            if (!File.Exists(this._salesData))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "DataSource"));
                this.CreateFile(this._salesData);
            }
            List<Sale> saleList1 = new List<Sale>();
            List<Sale> saleList2;
            using (StreamReader streamReader = new StreamReader(this._containersData))
                saleList2 = (List<Sale>)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(List<Sale>));
            if (saleList2 == null)
                return new List<Sale>();
            foreach (Sale sale in saleList2)
            {
                if (sale.Status == "FirstWeight")
                    saleList1.Add(sale);
            }
            return saleList1;
        }

        internal void UpdateWeigbridgeFile(int purchaseNumber, int wbNumber, int saleNumber)
        {
            if (!File.Exists(this._wbCounter))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "DataSource"));
                this.CreateFile(this._wbCounter);
            }
            int num1 = 0;
            int num2 = 0;
            using (Stream stream = (Stream)File.Open(this._wbCounter, FileMode.Open))
            {
                using (TextReader textReader = (TextReader)new StreamReader(stream, Encoding.UTF8))
                {
                    string str;
                    while ((str = textReader.ReadLine()) != null)
                    {
                        string[] strArray = str.Split(' ');
                        num1 = int.Parse(strArray[0]);
                        num2 = int.Parse(strArray[2]);
                    }
                }
            }
            if (purchaseNumber > num1 && saleNumber == 0)
            {
                string str = string.Format("{0} {1} {2}", (object)purchaseNumber, (object)wbNumber, (object)num2);
                using (StreamWriter streamWriter = new StreamWriter(this._wbCounter))
                    streamWriter.WriteLine(str);
            }
            if (purchaseNumber != 0 || saleNumber <= num2)
                return;
            string str1 = string.Format("{0} {1} {2}", (object)num1, (object)wbNumber, (object)saleNumber);
            using (StreamWriter streamWriter = new StreamWriter(this._wbCounter))
                streamWriter.WriteLine(str1);
        }

        internal void RecoverWeighBridges()
        {
            if (File.Exists(this._purchaseData))
            {
                List<Purchase> purchaseList;
                using (StreamReader streamReader = new StreamReader(this._purchaseData))
                    purchaseList = (List<Purchase>)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(List<Purchase>));
                if (purchaseList != null)
                {
                    foreach (Purchase purchase in purchaseList)
                    {
                        if (purchase.Status == "UnProcessed")
                        {
                            using (DataClassService dataClassService = new DataClassService())
                                dataClassService.RecoverPurchase(purchase);
                        }
                    }
                    using (FileStream fileStream = File.Open(this._purchaseData, FileMode.Open))
                        fileStream.SetLength(0L);
                }
            }
            if (!File.Exists(this._salesData))
                return;
            List<Sale> saleList;
            using (StreamReader streamReader = new StreamReader(this._salesData))
                saleList = (List<Sale>)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(List<Purchase>));
            if (saleList != null)
            {
                foreach (Sale sale in saleList)
                {
                    if (sale.Status == "UnProcessed")
                    {
                        using (DataClassService dataClassService = new DataClassService())
                            dataClassService.RecoverSale(sale);
                    }
                }
                using (FileStream fileStream = File.Open(this._salesData, FileMode.Open))
                    fileStream.SetLength(0L);
            }
        }

        internal void AddPurchase(Purchase purchase)
        {
            if (!File.Exists(this._wbCounter))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "DataSource"));
                this.CreateFile(this._wbCounter);
            }
            int purchaseNumber = 0;
            int wbNumber = 0;
            using (Stream stream = (Stream)File.Open(this._wbCounter, FileMode.Open))
            {
                using (TextReader textReader = (TextReader)new StreamReader(stream, Encoding.UTF8))
                {
                    string str;
                    while ((str = textReader.ReadLine()) != null)
                    {
                        string[] strArray = str.Split(' ');
                        purchaseNumber = int.Parse(strArray[0]) + 1;
                        wbNumber = int.Parse(strArray[1]) + 1;
                    }
                }
            }
            purchase.Id = purchaseNumber;
            purchase.WeighBridgeInfo.Id = wbNumber;
            List<Purchase> purchaseList;
            using (StreamReader streamReader = new StreamReader(this._purchaseData))
            {
                string end = streamReader.ReadToEnd();
                purchaseList = !string.IsNullOrEmpty(end) ? (List<Purchase>)JsonConvert.DeserializeObject(end, typeof(List<Purchase>)) : new List<Purchase>();
            }
            using (FileStream fileStream = File.Open(this._purchaseData, FileMode.Open))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream))
                {
                    using (JsonWriter jsonWriter = (JsonWriter)new JsonTextWriter((TextWriter)streamWriter))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        JsonSerializer jsonSerializer = new JsonSerializer();
                        purchaseList.Add(purchase);
                        jsonSerializer.Serialize(jsonWriter, (object)purchaseList);
                    }
                }
            }
            this.UpdateWeigbridgeFile(purchaseNumber, wbNumber, 0);
        }

        internal void UpdatePurchase(Purchase purchase)
        {
            List<Purchase> source;
            using (StreamReader streamReader = new StreamReader(this._purchaseData))
                source = (List<Purchase>)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(List<Purchase>));
            if (source != null)
            {
                Purchase purchase1 = source.FirstOrDefault<Purchase>((Func<Purchase, bool>)(x => x.WeighBridgeInfo.Id == purchase.WeighBridgeInfo.Id));
                purchase1.WeighBridgeInfo.SecondMass = purchase.WeighBridgeInfo.SecondMass;
                purchase1.WeighBridgeInfo.DateOut = purchase.WeighBridgeInfo.DateOut;
                purchase1.Status = "UnProcessed";
                purchase1.WeighBridgeInfo.NettMass = purchase.WeighBridgeInfo.NettMass;
                purchase1.Price = purchase.Price;
                purchase1.TotalPrice = purchase.TotalPrice;
                purchase1.WeighBridgeInfo.Product = purchase.WeighBridgeInfo.Product;
                purchase1.WeighBridgeInfo.Comments = purchase.WeighBridgeInfo.Comments;
            }
            using (FileStream fileStream = File.Open(this._purchaseData, FileMode.Open))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream))
                {
                    using (JsonWriter jsonWriter = (JsonWriter)new JsonTextWriter((TextWriter)streamWriter))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        new JsonSerializer().Serialize(jsonWriter, (object)source);
                    }
                }
            }
        }

        internal void AddSale(Sale sale)
        {
            if (!File.Exists(this._wbCounter))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "DataSource"));
                this.CreateFile(this._wbCounter);
            }
            int saleNumber = 0;
            int wbNumber = 0;
            using (Stream stream = (Stream)File.Open(this._wbCounter, FileMode.Open))
            {
                using (TextReader textReader = (TextReader)new StreamReader(stream, Encoding.UTF8))
                {
                    string str;
                    while ((str = textReader.ReadLine()) != null)
                    {
                        string[] strArray = str.Split(' ');
                        saleNumber = int.Parse(strArray[0]) + 1;
                        wbNumber = int.Parse(strArray[1]) + 1;
                    }
                }
            }
            sale.Id = saleNumber;
            sale.WeighBridgeInfo.Id = wbNumber;
            List<Sale> saleList;
            using (StreamReader streamReader = new StreamReader(this._salesData))
            {
                string end = streamReader.ReadToEnd();
                saleList = !string.IsNullOrEmpty(end) ? (List<Sale>)JsonConvert.DeserializeObject(end, typeof(List<Sale>)) : new List<Sale>();
            }
            using (FileStream fileStream = File.Open(this._salesData, FileMode.Open))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream))
                {
                    using (JsonWriter jsonWriter = (JsonWriter)new JsonTextWriter((TextWriter)streamWriter))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        JsonSerializer jsonSerializer = new JsonSerializer();
                        saleList.Add(sale);
                        jsonSerializer.Serialize(jsonWriter, (object)saleList);
                    }
                }
            }
            this.UpdateWeigbridgeFile(0, wbNumber, saleNumber);
        }

        internal void UpdateSale(Sale sale)
        {
            List<Sale> source;
            using (StreamReader streamReader = new StreamReader(this._purchaseData))
                source = (List<Sale>)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(List<Sale>));
            if (source != null)
            {
                Sale sale1 = source.FirstOrDefault<Sale>((Func<Sale, bool>)(x => x.WeighBridgeInfo.Id == sale.WeighBridgeInfo.Id));
                sale1.WeighBridgeInfo.SecondMass = sale.WeighBridgeInfo.SecondMass;
                sale1.WeighBridgeInfo.DateOut = sale.WeighBridgeInfo.DateOut;
                sale1.Status = "UnProcessed";
                sale1.WeighBridgeInfo.NettMass = sale.WeighBridgeInfo.NettMass;
                sale1.WeighBridgeInfo.Product = sale.WeighBridgeInfo.Product;
                sale1.WeighBridgeInfo.Comments = sale.WeighBridgeInfo.Comments;
            }
            using (FileStream fileStream = File.Open(this._purchaseData, FileMode.Open))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream))
                {
                    using (JsonWriter jsonWriter = (JsonWriter)new JsonTextWriter((TextWriter)streamWriter))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        new JsonSerializer().Serialize(jsonWriter, (object)source);
                    }
                }
            }
        }
    }
}
