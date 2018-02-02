// Decompiled with JetBrains decompiler
// Type: WeighBridgeApplication.ViewModel.ViewModelBase
// Assembly: WeighBridgeApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0008ECA4-ECDC-4057-A6F3-F7FD4CE2F23F
// Assembly location: C:\Users\Ndavhe\Desktop\Rustivia Weight Bridge\WeighBridgeApplication.exe

using System;
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
using WeighBridgeApplication.OfflineMode;
using WeighBridgeApplication.Util;

namespace WeighBridgeApplication.ViewModel
{
    public class ViewModelBase : PropertyChangedNotify
    {
        public static string SystemUser;
        private ReadOnlyObservableCollection<Customer> _customers;
        private ReadOnlyObservableCollection<Driver> _drivers;
        private ReadOnlyObservableCollection<Product> _productList;
        private ReadOnlyObservableCollection<Supplier> _suppliers;
        private ReadOnlyObservableCollection<Truck> _trucks;

        protected ReadOnlyObservableCollection<Truck> Trucks
        {
            get
            {
                return this._trucks;
            }
            set
            {
                if (this._trucks == value)
                    return;
                this._trucks = value;
                this.OnPropertyChanged(nameof(Trucks));
            }
        }

        protected ReadOnlyObservableCollection<Driver> Drivers
        {
            get
            {
                return this._drivers;
            }
            set
            {
                if (this._drivers == value)
                    return;
                this._drivers = value;
                this.OnPropertyChanged(nameof(Drivers));
            }
        }

        protected ReadOnlyObservableCollection<Supplier> Suppliers
        {
            get
            {
                return this._suppliers;
            }
            set
            {
                if (this._suppliers == value)
                    return;
                this._suppliers = value;
                this.OnPropertyChanged(nameof(Suppliers));
            }
        }

        protected ReadOnlyObservableCollection<Product> Products
        {
            get
            {
                return this._productList;
            }
            set
            {
                if (this._productList == value)
                    return;
                this._productList = value;
                this.OnPropertyChanged(nameof(Products));
            }
        }

        protected ReadOnlyObservableCollection<Customer> Customers
        {
            get
            {
                return this._customers;
            }
            set
            {
                if (this._customers == value)
                    return;
                this._customers = value;
                this.OnPropertyChanged(nameof(Customers));
            }
        }

        public Product ViewBaseProduct { get; set; }

        public Supplier ViewBaseSupplier { get; set; }

        public ObservableCollection<Customer> GetCustomers()
        {
            if (!Program.IsServiceDown)
            {
                using (DataClassService dataClassService = new DataClassService())
                {
                    ObservableCollection<Customer> observableCollection = new ObservableCollection<Customer>();
                    EventLogHelper.Log("Calling  getCustomers Service");
                    foreach (Customer customer in dataClassService.GetCustomers())
                        observableCollection.Add(customer);
                    return observableCollection;
                }
            }
            else
            {
                using (DataClassJson dataClassJson = new DataClassJson())
                {
                    ObservableCollection<Customer> observableCollection = new ObservableCollection<Customer>();
                    EventLogHelper.Log("Calling  getCustomers Service");
                    foreach (Customer loadCustomer in dataClassJson.LoadCustomers())
                        observableCollection.Add(loadCustomer);
                    return observableCollection;
                }
            }
        }

        public ObservableCollection<Truck> GetTrucks()
        {
            if (!Program.IsServiceDown)
            {
                using (DataClassService dataClassService = new DataClassService())
                {
                    ObservableCollection<Truck> observableCollection = new ObservableCollection<Truck>();
                    EventLogHelper.Log("Calling  getTrucks Service");
                    foreach (Truck truck in dataClassService.GetTrucks())
                        observableCollection.Add(truck);
                    return observableCollection;
                }
            }
            else
            {
                using (DataClassJson dataClassJson = new DataClassJson())
                {
                    ObservableCollection<Truck> observableCollection = new ObservableCollection<Truck>();
                    EventLogHelper.Log("Calling  getTrucks Service");
                    foreach (Truck loadTruck in (IEnumerable<Truck>)dataClassJson.LoadTrucks())
                        observableCollection.Add(loadTruck);
                    return observableCollection;
                }
            }
        }

        public ObservableCollection<Supplier> GetSuppliers()
        {
            if (!Program.IsServiceDown)
            {
                using (DataClassService dataClassService = new DataClassService())
                {
                    ObservableCollection<Supplier> observableCollection = new ObservableCollection<Supplier>();
                    EventLogHelper.Log("Calling  getSuppliers Service");
                    foreach (Supplier supplier in dataClassService.GetSuppliers())
                        observableCollection.Add(supplier);
                    return observableCollection;
                }
            }
            else
            {
                using (DataClassJson dataClassJson = new DataClassJson())
                {
                    ObservableCollection<Supplier> observableCollection = new ObservableCollection<Supplier>();
                    EventLogHelper.Log("Calling  getSuppliers Service");
                    foreach (Supplier loadSupplier in (IEnumerable<Supplier>)dataClassJson.LoadSuppliers())
                        observableCollection.Add(loadSupplier);
                    return observableCollection;
                }
            }
        }

        public ObservableCollection<Product> GetProducts()
        {
            if (!Program.IsServiceDown)
            {
                using (DataClassService dataClassService = new DataClassService())
                {
                    ObservableCollection<Product> observableCollection = new ObservableCollection<Product>();
                    EventLogHelper.Log("Calling  getSuppliers Service");
                    foreach (Product product in dataClassService.GetProducts())
                        observableCollection.Add(product);
                    return observableCollection;
                }
            }
            else
            {
                using (DataClassJson dataClassJson = new DataClassJson())
                {
                    ObservableCollection<Product> observableCollection = new ObservableCollection<Product>();
                    EventLogHelper.Log("Calling  getSuppliers Service");
                    foreach (Product loadProduct in dataClassJson.LoadProducts())
                        observableCollection.Add(loadProduct);
                    return observableCollection;
                }
            }
        }

        public ObservableCollection<Driver> GetDrivers()
        {
            if (!Program.IsServiceDown)
            {
                using (DataClassService dataClassService = new DataClassService())
                {
                    ObservableCollection<Driver> observableCollection = new ObservableCollection<Driver>();
                    EventLogHelper.Log("Calling  getDrivers Service");
                    foreach (Driver driver in dataClassService.GetDrivers())
                        observableCollection.Add(driver);
                    return observableCollection;
                }
            }
            else
            {
                using (DataClassJson dataClassJson = new DataClassJson())
                {
                    ObservableCollection<Driver> observableCollection = new ObservableCollection<Driver>();
                    EventLogHelper.Log("Calling  getDrivers Service");
                    foreach (Driver loadDriver in (IEnumerable<Driver>)dataClassJson.LoadDrivers())
                        observableCollection.Add(loadDriver);
                    return observableCollection;
                }
            }
        }

        protected void PrintWeightBridgeReceipt(Purchase purchase, Supplier selectedSupplier, string type)
        {
            Supplier supplier = selectedSupplier;
            PrintDialog printDialog = new PrintDialog();
            if (!printDialog.ShowDialog().GetValueOrDefault())
                return;
            FlowDocument flowDocument = new FlowDocument()
            {
                PageWidth = 320.0
            };
            Paragraph paragraph1 = new Paragraph();
            Thickness thickness1 = new Thickness(0.0);
            paragraph1.Margin = thickness1;
            FontFamily fontFamily1 = new FontFamily("Arial");
            paragraph1.FontFamily = fontFamily1;
            double num1 = 18.0;
            paragraph1.FontSize = num1;
            Thickness thickness2 = new Thickness(1.0, 0.0, 1.0, 0.0);
            paragraph1.Padding = thickness2;
            Paragraph paragraph2 = paragraph1;
            paragraph2.Inlines.Add("Rustivia Metals CC");
            flowDocument.Blocks.Add((Block)paragraph2);
            Paragraph paragraph3 = new Paragraph();
            Thickness thickness3 = new Thickness(0.0);
            paragraph3.Margin = thickness3;
            FontFamily fontFamily2 = new FontFamily("Arial");
            paragraph3.FontFamily = fontFamily2;
            double num2 = 12.0;
            paragraph3.FontSize = num2;
            Thickness thickness4 = new Thickness(1.0, 3.0, 1.0, 0.0);
            paragraph3.Padding = thickness4;
            Paragraph paragraph4 = paragraph3;
            Paragraph paragraph5 = new Paragraph();
            Thickness thickness5 = new Thickness(0.0);
            paragraph5.Margin = thickness5;
            FontFamily fontFamily3 = new FontFamily("Arial");
            paragraph5.FontFamily = fontFamily3;
            double num3 = 12.0;
            paragraph5.FontSize = num3;
            Thickness thickness6 = new Thickness(1.0, 0.0, 1.0, 0.0);
            paragraph5.Padding = thickness6;
            Paragraph paragraph6 = paragraph5;
            paragraph6.Inlines.Add("54 Northreef Rd");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("Activia park");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("Germiston, South Africa");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("(T) 011 828 9961");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("Reg no. 1997/0025504/23");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("Vat no. 4610216634");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            Line line1 = new Line();
            int num4 = 1;
            line1.Stretch = (Stretch)num4;
            SolidColorBrush black1 = Brushes.Black;
            line1.Stroke = (Brush)black1;
            double num5 = 1.0;
            line1.X2 = num5;
            Thickness thickness7 = new Thickness(0.0, 0.0, 30.0, 0.0);
            line1.Margin = thickness7;
            Line line2 = line1;
            paragraph6.Inlines.Add((UIElement)line2);
            flowDocument.Blocks.Add((Block)paragraph6);
            string text1 = "W/Bridge No:".PadRight(17) + purchase.WeighBridgeInfo.Id.ToString((IFormatProvider)CultureInfo.InvariantCulture);
            string text2 = "Registration No:".PadRight(18) + purchase.Truck.TruckRegNumber;
            string str1 = "Date In:".PadRight(22);
            DateTime dateTime = purchase.WeighBridgeInfo.DateIn;
            string str2 = dateTime.ToString("yyyy-MM-dd hh:mm:ss");
            string text3 = str1 + str2;
            string str3 = "Date Out:".PadRight(20);
            dateTime = purchase.WeighBridgeInfo.DateOut;
            string str4 = dateTime.ToString("yyyy-MM-dd hh:mm:ss");
            string text4 = str3 + str4;
            string text5 = "Supplier: ".PadRight(15) + (supplier != null ? supplier.SupplierName : (string)null);
            string text6 = "Supplier Code: ".PadRight(19) + (supplier != null ? supplier.Suppliercode : (string)null);
            string text7 = "Driver:".PadRight(15) + string.Format("{0} {1}", (object)purchase.Driver.Surname, (object)purchase.Driver.Firstname);
            string text8 = "Product:".PadRight(22) + purchase.WeighBridgeInfo.Product;
            string text9 = "Comments:".PadRight(18) + purchase.WeighBridgeInfo.Comments;
            string text10 = "First Mass:".PadRight(22) + string.Format("{0} KG", (object)purchase.WeighBridgeInfo.FirstMass.ToString((IFormatProvider)CultureInfo.InvariantCulture));
            string text11 = "Second Mass:".PadRight(18) + string.Format("{0} KG", (object)purchase.WeighBridgeInfo.SecondMass.ToString((IFormatProvider)CultureInfo.InvariantCulture));
            string text12 = "Nett Mass:".PadRight(22) + string.Format("{0} KG", (object)purchase.WeighBridgeInfo.NettMass.ToString((IFormatProvider)CultureInfo.InvariantCulture));
            Decimal num6 = this.ViewBaseSupplier.SupplierProducts.FirstOrDefault<SupplierProduct>((Func<SupplierProduct, bool>)(sp => sp.ProductId == this.ViewBaseProduct.Id)) != null ? this.ViewBaseSupplier.SupplierProducts.FirstOrDefault<SupplierProduct>((Func<SupplierProduct, bool>)(sp => sp.ProductId == this.ViewBaseProduct.Id)).SupplierPrice : this.ViewBaseProduct.RustiviaPrice;
            string text13 = "Price:".PadRight(24) + string.Format("R {0} ", (object)((Decimal)purchase.WeighBridgeInfo.NettMass * num6));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(type));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text1));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text2));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text3));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text4));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text5));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text6));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text7));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text8));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text9));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text10));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text11));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            Line line3 = new Line();
            int num7 = 1;
            line3.Stretch = (Stretch)num7;
            SolidColorBrush black2 = Brushes.Black;
            line3.Stroke = (Brush)black2;
            double num8 = 1.0;
            line3.X2 = num8;
            Thickness thickness8 = new Thickness(0.0, 0.0, 10.0, 0.0);
            line3.Margin = thickness8;
            Line line4 = line3;
            paragraph4.Inlines.Add((UIElement)line4);
            paragraph4.Inlines.Add((Inline)new Run(text12));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text13));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            Line line5 = new Line();
            int num9 = 1;
            line5.Stretch = (Stretch)num9;
            SolidColorBrush white = Brushes.White;
            line5.Stroke = (Brush)white;
            double num10 = 1.0;
            line5.X2 = num10;
            Thickness thickness9 = new Thickness(0.0, 40.0, 50.0, 0.0);
            line5.Margin = thickness9;
            Line line6 = line5;
            paragraph4.Inlines.Add((UIElement)line6);
            paragraph4.Inlines.Add((Inline)new Run("Weighbridge Clerk:".PadRight(22) + ViewModelBase.SystemUser));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            Line line7 = new Line();
            int num11 = 1;
            line7.Stretch = (Stretch)num11;
            SolidColorBrush black3 = Brushes.Black;
            line7.Stroke = (Brush)black3;
            double num12 = 1.0;
            line7.X2 = num12;
            Thickness thickness10 = new Thickness(0.0, 40.0, 80.0, 0.0);
            line7.Margin = thickness10;
            Line line8 = line7;
            paragraph4.Inlines.Add((UIElement)line8);
            paragraph4.Inlines.Add((Inline)new Run("Driver signature:"));
            flowDocument.Blocks.Add((Block)paragraph4);
            DocumentPaginator documentPaginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
            printDialog.PrintDocument(documentPaginator, "Invoice");
        }

        protected void PrintSaleReceipt(Sale sale, string type)
        {
            this.Customers = new ReadOnlyObservableCollection<Customer>(this.GetCustomers());
            Customer customer = this.Customers.SingleOrDefault<Customer>((Func<Customer, bool>)(a => a.Id == sale.Customer.Id));
            PrintDialog printDialog = new PrintDialog();
            if (!printDialog.ShowDialog().GetValueOrDefault())
                return;
            FlowDocument flowDocument = new FlowDocument()
            {
                PageWidth = 320.0
            };
            Paragraph paragraph1 = new Paragraph();
            Thickness thickness1 = new Thickness(0.0);
            paragraph1.Margin = thickness1;
            FontFamily fontFamily1 = new FontFamily("Arial");
            paragraph1.FontFamily = fontFamily1;
            double num1 = 18.0;
            paragraph1.FontSize = num1;
            Thickness thickness2 = new Thickness(1.0, 0.0, 1.0, 0.0);
            paragraph1.Padding = thickness2;
            Paragraph paragraph2 = paragraph1;
            paragraph2.Inlines.Add("Rustivia Metals CC");
            flowDocument.Blocks.Add((Block)paragraph2);
            Paragraph paragraph3 = new Paragraph();
            Thickness thickness3 = new Thickness(0.0);
            paragraph3.Margin = thickness3;
            FontFamily fontFamily2 = new FontFamily("Arial");
            paragraph3.FontFamily = fontFamily2;
            double num2 = 12.0;
            paragraph3.FontSize = num2;
            Thickness thickness4 = new Thickness(1.0, 3.0, 1.0, 0.0);
            paragraph3.Padding = thickness4;
            Paragraph paragraph4 = paragraph3;
            Paragraph paragraph5 = new Paragraph();
            Thickness thickness5 = new Thickness(0.0);
            paragraph5.Margin = thickness5;
            FontFamily fontFamily3 = new FontFamily("Arial");
            paragraph5.FontFamily = fontFamily3;
            double num3 = 12.0;
            paragraph5.FontSize = num3;
            Thickness thickness6 = new Thickness(1.0, 0.0, 1.0, 0.0);
            paragraph5.Padding = thickness6;
            Paragraph paragraph6 = paragraph5;
            paragraph6.Inlines.Add("54 Northreef");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("Activia perk");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("Germiston, South Africa");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("(T) 011 828 9961");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("Reg no. 1997/0025504/23");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("Vat no. 4610216634");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            Line line1 = new Line();
            int num4 = 1;
            line1.Stretch = (Stretch)num4;
            SolidColorBrush black1 = Brushes.Black;
            line1.Stroke = (Brush)black1;
            double num5 = 1.0;
            line1.X2 = num5;
            Thickness thickness7 = new Thickness(0.0, 0.0, 30.0, 0.0);
            line1.Margin = thickness7;
            Line line2 = line1;
            paragraph6.Inlines.Add((UIElement)line2);
            flowDocument.Blocks.Add((Block)paragraph6);
            string text1 = "W/Bridge No:".PadRight(17) + sale.WeighBridgeInfo.Id.ToString((IFormatProvider)CultureInfo.InvariantCulture);
            string text2 = "Registration No:".PadRight(18) + sale.TruckRegNumber;
            string text3 = "Date In:".PadRight(22) + sale.WeighBridgeInfo.DateIn.ToString("yyyy-MM-dd hh:mm:ss");
            string text4 = "Date Out:".PadRight(20) + sale.WeighBridgeInfo.DateOut.ToString("yyyy-MM-dd hh:mm:ss");
            string text5 = "Customer: ".PadRight(15) + (customer != null ? customer.CustomerName : (string)null);
            string text6 = "Customer Code: ".PadRight(19) + (customer != null ? customer.CustomerCode : (string)null);
            string text7 = "Product:".PadRight(22) + sale.WeighBridgeInfo.Product;
            string text8 = "Extra Info:".PadRight(18) + sale.ExtraInfo;
            string str1 = "First Mass:".PadRight(22);
            string format1 = "{0} KG";
            long num6 = sale.WeighBridgeInfo.FirstMass;
            string str2 = num6.ToString((IFormatProvider)CultureInfo.InvariantCulture);
            string str3 = string.Format(format1, (object)str2);
            string text9 = str1 + str3;
            string str4 = "Second Mass:".PadRight(18);
            string format2 = "{0} KG";
            num6 = sale.WeighBridgeInfo.SecondMass;
            string str5 = num6.ToString((IFormatProvider)CultureInfo.InvariantCulture);
            string str6 = string.Format(format2, (object)str5);
            string text10 = str4 + str6;
            string str7 = "Nett Mass:".PadRight(22);
            string format3 = "{0} KG";
            num6 = sale.WeighBridgeInfo.NettMass;
            string str8 = num6.ToString((IFormatProvider)CultureInfo.InvariantCulture);
            string str9 = string.Format(format3, (object)str8);
            string text11 = str7 + str9;
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(type));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text1));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text2));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text3));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text4));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text5));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text6));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text7));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text8));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text9));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text10));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            Line line3 = new Line();
            int num7 = 1;
            line3.Stretch = (Stretch)num7;
            SolidColorBrush black2 = Brushes.Black;
            line3.Stroke = (Brush)black2;
            double num8 = 1.0;
            line3.X2 = num8;
            Thickness thickness8 = new Thickness(0.0, 0.0, 10.0, 0.0);
            line3.Margin = thickness8;
            Line line4 = line3;
            paragraph4.Inlines.Add((UIElement)line4);
            paragraph4.Inlines.Add((Inline)new Run(text11));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            Line line5 = new Line();
            int num9 = 1;
            line5.Stretch = (Stretch)num9;
            SolidColorBrush white = Brushes.White;
            line5.Stroke = (Brush)white;
            double num10 = 1.0;
            line5.X2 = num10;
            Thickness thickness9 = new Thickness(0.0, 40.0, 50.0, 0.0);
            line5.Margin = thickness9;
            Line line6 = line5;
            paragraph4.Inlines.Add((UIElement)line6);
            paragraph4.Inlines.Add((Inline)new Run("Weighbridge Clerk:".PadRight(22) + ViewModelBase.SystemUser));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            Line line7 = new Line();
            int num11 = 1;
            line7.Stretch = (Stretch)num11;
            SolidColorBrush black3 = Brushes.Black;
            line7.Stroke = (Brush)black3;
            double num12 = 1.0;
            line7.X2 = num12;
            Thickness thickness10 = new Thickness(0.0, 40.0, 80.0, 0.0);
            line7.Margin = thickness10;
            Line line8 = line7;
            paragraph4.Inlines.Add((UIElement)line8);
            paragraph4.Inlines.Add((Inline)new Run("Driver signature:"));
            flowDocument.Blocks.Add((Block)paragraph4);
            DocumentPaginator documentPaginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
            printDialog.PrintDocument(documentPaginator, "Invoice");
        }

        protected void PrintContainerReceipt(Container containerDto, string type, Customer selectedCustomer)
        {
            Customer customer = selectedCustomer;
            string referenceNumber = containerDto.Booking.ReferenceNumber;
            PrintDialog printDialog = new PrintDialog();
            if (!printDialog.ShowDialog().GetValueOrDefault())
                return;
            FlowDocument flowDocument = new FlowDocument()
            {
                PageWidth = 320.0
            };
            Paragraph paragraph1 = new Paragraph();
            Thickness thickness1 = new Thickness(0.0);
            paragraph1.Margin = thickness1;
            FontFamily fontFamily1 = new FontFamily("Arial");
            paragraph1.FontFamily = fontFamily1;
            double num1 = 18.0;
            paragraph1.FontSize = num1;
            Thickness thickness2 = new Thickness(1.0, 0.0, 1.0, 0.0);
            paragraph1.Padding = thickness2;
            Paragraph paragraph2 = paragraph1;
            paragraph2.Inlines.Add("Rustivia Metals CC");
            flowDocument.Blocks.Add((Block)paragraph2);
            Paragraph paragraph3 = new Paragraph();
            Thickness thickness3 = new Thickness(0.0);
            paragraph3.Margin = thickness3;
            FontFamily fontFamily2 = new FontFamily("Arial");
            paragraph3.FontFamily = fontFamily2;
            double num2 = 12.0;
            paragraph3.FontSize = num2;
            Thickness thickness4 = new Thickness(1.0, 3.0, 1.0, 0.0);
            paragraph3.Padding = thickness4;
            Paragraph paragraph4 = paragraph3;
            Paragraph paragraph5 = new Paragraph();
            Thickness thickness5 = new Thickness(0.0);
            paragraph5.Margin = thickness5;
            FontFamily fontFamily3 = new FontFamily("Arial");
            paragraph5.FontFamily = fontFamily3;
            double num3 = 12.0;
            paragraph5.FontSize = num3;
            Thickness thickness6 = new Thickness(1.0, 0.0, 1.0, 0.0);
            paragraph5.Padding = thickness6;
            Paragraph paragraph6 = paragraph5;
            paragraph6.Inlines.Add("54 Northreef");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("Activia perk");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("Germiston, South Africa");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("(T) 011 828 9961");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("Reg no. 1997/0025504/23");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            paragraph6.Inlines.Add("Vat no. 4610216634");
            paragraph6.Inlines.Add((Inline)new LineBreak());
            Line line1 = new Line();
            int num4 = 1;
            line1.Stretch = (Stretch)num4;
            SolidColorBrush black1 = Brushes.Black;
            line1.Stroke = (Brush)black1;
            double num5 = 1.0;
            line1.X2 = num5;
            Thickness thickness7 = new Thickness(0.0, 0.0, 30.0, 0.0);
            line1.Margin = thickness7;
            Line line2 = line1;
            paragraph6.Inlines.Add((UIElement)line2);
            flowDocument.Blocks.Add((Block)paragraph6);
            string text1 = "W/Bridge No:".PadRight(17) + containerDto.WeighBridgeInfo.Id.ToString((IFormatProvider)CultureInfo.InvariantCulture);
            string text2 = "Reg No:".PadRight(18) + containerDto.TruckRegNumber;
            string text3 = "Seal no:".PadRight(18) + containerDto.Sealnumber;
            string text4 = "Container No:".PadRight(18) + containerDto.ContainerNumber;
            string text5 = "Date In:".PadRight(22) + containerDto.WeighBridgeInfo.DateIn.ToString("yyyy-MM-dd hh:mm:ss");
            string text6 = "Date Out:".PadRight(20) + containerDto.WeighBridgeInfo.DateOut.ToString("yyyy-MM-dd hh:mm:ss");
            string text7 = "Customer: ".PadRight(15) + (customer != null ? customer.CustomerName : (string)null);
            string text8 = "Tare:".PadRight(23) + string.Format("{0} KG", (object)containerDto.TareWeight);
            string text9 = "Product:".PadRight(22) + containerDto.Product;
            string text10 = "Reference No:".PadRight(18) + referenceNumber;
            string str1 = "First Mass:".PadRight(22);
            string format1 = "{0} KG";
            long num6 = containerDto.WeighBridgeInfo.FirstMass;
            string str2 = num6.ToString((IFormatProvider)CultureInfo.InvariantCulture);
            string str3 = string.Format(format1, (object)str2);
            string text11 = str1 + str3;
            string str4 = "Second Mass:".PadRight(18);
            string format2 = "{0} KG";
            num6 = containerDto.WeighBridgeInfo.SecondMass;
            string str5 = num6.ToString((IFormatProvider)CultureInfo.InvariantCulture);
            string str6 = string.Format(format2, (object)str5);
            string text12 = str4 + str6;
            string str7 = "Nett Mass:".PadRight(22);
            string format3 = "{0} KG";
            num6 = containerDto.WeighBridgeInfo.NettMass;
            string str8 = num6.ToString((IFormatProvider)CultureInfo.InvariantCulture);
            string str9 = string.Format(format3, (object)str8);
            string text13 = str7 + str9;
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(type));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text1));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text10));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text2));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text3));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text4));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text5));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text6));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text7));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text9));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text11));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text12));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new Run(text8));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            paragraph4.Inlines.Add((Inline)new LineBreak());
            Line line3 = new Line();
            int num7 = 1;
            line3.Stretch = (Stretch)num7;
            SolidColorBrush black2 = Brushes.Black;
            line3.Stroke = (Brush)black2;
            double num8 = 1.0;
            line3.X2 = num8;
            Thickness thickness8 = new Thickness(0.0, 0.0, 10.0, 0.0);
            line3.Margin = thickness8;
            Line line4 = line3;
            paragraph4.Inlines.Add((UIElement)line4);
            paragraph4.Inlines.Add((Inline)new Run(text13));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            Line line5 = new Line();
            int num9 = 1;
            line5.Stretch = (Stretch)num9;
            SolidColorBrush white = Brushes.White;
            line5.Stroke = (Brush)white;
            double num10 = 1.0;
            line5.X2 = num10;
            Thickness thickness9 = new Thickness(0.0, 40.0, 50.0, 0.0);
            line5.Margin = thickness9;
            Line line6 = line5;
            paragraph4.Inlines.Add((UIElement)line6);
            paragraph4.Inlines.Add((Inline)new Run("Weighbridge Clerk:".PadRight(22) + ViewModelBase.SystemUser));
            paragraph4.Inlines.Add((Inline)new LineBreak());
            Line line7 = new Line();
            int num11 = 1;
            line7.Stretch = (Stretch)num11;
            SolidColorBrush black3 = Brushes.Black;
            line7.Stroke = (Brush)black3;
            double num12 = 1.0;
            line7.X2 = num12;
            Thickness thickness10 = new Thickness(0.0, 40.0, 80.0, 0.0);
            line7.Margin = thickness10;
            Line line8 = line7;
            paragraph4.Inlines.Add((UIElement)line8);
            paragraph4.Inlines.Add((Inline)new Run("Driver signature:"));
            flowDocument.Blocks.Add((Block)paragraph4);
            DocumentPaginator documentPaginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
            printDialog.PrintDocument(documentPaginator, "Invoice");
        }
    }
}
