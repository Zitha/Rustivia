using System;
using System.Collections.Generic;

namespace WeighBridgeApplication.Model
{
    [Serializable]
    public class Booking
    {
        public string ReferenceNumber { get; set; }

        public List<Container> Containers { get; set; }
    }
}