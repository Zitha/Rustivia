using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeighBridgeApplication.Model
{
    public class Purchase
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public Truck Truck { get; set; }

        public Driver Driver { get; set; }

        public WeighBridgeInfo WeighBridgeInfo { get; set; }

        public Decimal Price { get; set; }

        public Decimal TotalPrice { get; set; }
    }
}
