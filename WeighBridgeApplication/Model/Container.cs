using System;

namespace WeighBridgeApplication.Model
{
    public class Container
    {
        public string ContainerNumber { get; set; }

        public string Sealnumber { get; set; }

        public long GrossWeight { get; set; }

        public long TareWeight { get; set; }

        public long NettWeight { get; set; }

        public string Product { get; set; }

        public string Status { get; set; }

        public string TruckRegNumber { get; set; }

        public DateTime DateIn { get; set; }

        public DateTime? DateOut { get; set; }

        public long FirstWeight { get; set; }

        public long SecondWeight { get; set; }

        public Booking Booking { get; set; }

        public WeighBridgeInfo WeighBridgeInfo { get; set; }
    }
}