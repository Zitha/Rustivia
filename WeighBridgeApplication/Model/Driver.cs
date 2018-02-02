using System;

namespace WeighBridgeApplication.Model
{
    [Serializable]
    public class Driver
    {
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string IdNumber { get; set; }

        public string Gender { get; set; }

        public string Surname { get; set; }

        public string IdLocation { get; set; }

        public string VatFormLocation { get; set; }

        public string ImageName { get; set; }

        public Supplier SupplierInfo { get; set; }

        public string FullName { get; set; }
    }
}