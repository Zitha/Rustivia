using System.Collections.Generic;

namespace WeighBridgeApplication.Model
{
    public class Supplier
    {
        public int Id { get; set; }

        public string SupplierName { get; set; }

        public string Logo { get; set; }

        public string Address { get; set; }

        public string TellNumber { get; set; }

        public string Suppliercode { get; set; }

        public string CompanyRegNumber { get; set; }

        public string VatNumber { get; set; }

        public string Legalnote { get; set; }

        public List<Driver> Drivers { get; set; }

        public List<Truck> Trucks { get; set; }
    }
}