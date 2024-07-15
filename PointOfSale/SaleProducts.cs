using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale
{
    public class SaleProducts
    {
        public  int SalesTransactionId { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public double TotalAmount { get; set; }

        public int Quantity { get; set; }
        
        public double ProductPrice { get; set; }
    
    }
  
}
