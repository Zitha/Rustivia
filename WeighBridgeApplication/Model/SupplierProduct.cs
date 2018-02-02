using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeighBridgeApplication.Model
{
    public class SupplierProduct
    {
        public Decimal SupplierPrice { get; set; }

        public int SupplierId { get; set; }

        public int ProductId { get; set; }
    }
}
