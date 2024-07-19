using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace PointOfSale.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public required string name { get; set; }
        public required double price { get; set; }
        public required int quantity { get; set; }

        public required string type { get; set; }
        public required string category { get; set; }
    }


}
