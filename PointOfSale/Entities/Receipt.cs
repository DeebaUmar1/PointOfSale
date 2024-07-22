using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Entities
{
    public class Receipt
    {
        public string Quantity { get; set; }
        public string Product { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
      
    }

    public class FinalReceipt
    {
        public List<Receipt> Receipt { get; set; }
        public string TotalAmount { get; set; }
    }

}
