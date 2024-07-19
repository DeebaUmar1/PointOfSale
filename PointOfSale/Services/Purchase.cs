using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using PointOfSale.Data;

namespace PointOfSale.Services
{
    public static class Purchase
    {
        public static List<Product> PurchaseProducts = new List<Product>()
        {
            new Product { Id = 3, name = "Monitor", price = 199.99, quantity = 20, type = "Electronics", category = "Computers" },
            new Product { Id = 4, name = "Keyboard", price = 49.99, quantity = 30, type = "Peripherals", category = "Accessories" },
            new Product { Id = 5, name = "Desk Lamp", price = 24.99, quantity = 15, type = "Furniture", category = "Office" },
            new Product { Id = 6, name = "USB Cable", price = 9.99, quantity = 100, type = "Peripherals", category = "Accessories" },
        };
        public static void ViewProducts()
        {
            Console.Clear();
            if (PurchaseProducts.Count == 0)
            {
                Console.WriteLine("No products available.");
            }
            else
            {
                Console.WriteLine("Products List:");
                Console.WriteLine(new string('-', 80));
                Console.WriteLine($"{"ID",-5} {"Name",-20} {"Price",-10} {"Quantity",-10} {"Type",-15} {"Category",-15}");
                Console.WriteLine(new string('-', 80));

                foreach (var product in PurchaseProducts)
                {
                    Console.WriteLine($"{product.Id,-5} {product.name,-20} {product.price,-10:C} {product.quantity,-10} {product.type,-15} {product.category,-15}");
                }

                Console.WriteLine(new string('-', 80));
            }

        }


        public static void AddProductToPurchase()
        {
            bool addMoreProducts = true;
            while (addMoreProducts)
            {
                Console.Clear();
                ViewProducts();
                Console.Write("Enter Product ID: ");
                var productId = Convert.ToInt32(Console.ReadLine());

                var product = PurchaseProducts.Find(p => p.Id == productId);
                if (product != null)
                {
                    Console.Write("Now enter the quantity: ");
                    var q = Convert.ToInt32(Console.ReadLine());
                    while (q < 0 || q > product.quantity)
                    {
                        Console.WriteLine("Invalid quantity!");
                        Console.Write("Enter valid quantity: ");
                        q = Convert.ToInt32(Console.ReadLine());
                    }

                    var sale = new SaleProducts
                    {
                        Date = DateTime.Now,
                        Quantity = q,
                        ProductId = productId,
                        ProductName = product.name,
                        ProductPrice = product.price
                    };
                    product.quantity -= q;
                    if (product.quantity == 0)
                    {
                        PurchaseProducts.Remove(product);
                    }
                    Transaction.Add(sale);
                    Console.WriteLine("Product added to purchase");
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }

                Console.Write("Do you want to add another product? (yes/no): ");
                string? response = Console.ReadLine();
                if (response != null)
                {
                    response = response.Trim().ToLower();
                    if (response != "yes")
                    {
                        addMoreProducts = false;
                    }
                }

            }
            Console.WriteLine("Purchase completed.");
            if (Transaction.SaleProducts.Count > 0)
            {
                Console.Write("Do you want to generate receipt? (y/n): ");
                string? response2 = Console.ReadLine();
                if (response2 != null)
                {
                    response2 = response2.Trim().ToLower();
                    if (response2 == "y")
                    {
                        //AddProductsToInventory();
                        GenerateReceipt();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                return;
            }



        }
        public static void CalculateTotalAmount()
        {
            if (Transaction.SaleProducts.Count > 0)
            {

                double total = Transaction.SaleProducts.Sum(s => s.Quantity * s.ProductPrice);
                Console.WriteLine($"Total Amount: {total:C}");
            }

        }

        public static void GenerateReceipt()
        {
            //Console.Clear();
            if (Transaction.SaleProducts.Count > 0)
            {

                Console.WriteLine("Receipt:");
                foreach (var sale in Transaction.SaleProducts)
                {
                    Console.WriteLine($"{sale.Quantity} x {sale.ProductName} @ {sale.ProductPrice:C} = {sale.Quantity * sale.ProductPrice:C}");
                }
                CalculateTotalAmount();

                AddProductsToInventory();

                Transaction.SaleProducts.Clear();
                //Console.ReadKey();
            }


        }

        public static void AddProductsToInventory()
        {
            foreach (var sale in Transaction.SaleProducts)
            {
                var product = PurchaseProducts.Find(p => p.Id == sale.ProductId);

                if (product != null)
                {
                    var newProduct = new Product
                    {

                        name = product.name,
                        price = product.price,
                        quantity = sale.Quantity,
                        type = product.type,
                        category = product.category

                    };
                    Inventory.Add(newProduct);
                }
            }
        }
    }
}
