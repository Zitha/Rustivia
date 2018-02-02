using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeighBridgeApplication.Model
{
    public class Sale
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public string TruckRegNumber { get; set; }

        public string ExtraInfo { get; set; }

        public WeighBridgeInfo WeighBridgeInfo { get; set; }

        public Customer Customer { get; set; }
    }
}
