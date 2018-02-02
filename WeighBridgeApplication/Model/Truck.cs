using System;

namespace WeighBridgeApplication.Model
{
    [Serializable]
    public class Truck
    {
        public int Id { get; set; }

        public string TruckRegNumber { get; set; }

        public bool Own { get; set; }
    }
}