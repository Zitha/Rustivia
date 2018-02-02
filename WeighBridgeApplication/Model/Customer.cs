using System;
using System.Collections.Generic;

namespace WeighBridgeApplication.Model
{
    [Serializable]
    public class Customer
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string TellNumber { get; set; }

        public string CustomerCode { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}