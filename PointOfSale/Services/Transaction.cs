using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointOfSale.Data;

namespace PointOfSale.Services
{
    public static class Transaction
    {
        public static List<SaleProducts> SaleProducts { get; set; } = new List<SaleProducts>();

        public static void Add(SaleProducts saleProducts)
        {
            SaleProducts.Add(saleProducts);
        }


    }
}
