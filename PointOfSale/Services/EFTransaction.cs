using Microsoft.EntityFrameworkCore;
using PointOfSale.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Services
{
    public static class EFTransaction
    {
        public static void Add(POSDbContext context, SaleProducts sales)
        {
            context.SaleProducts.Add(sales);
            context.SaveChanges();
        }

        public static void UpdateProductsInSale(POSDbContext context)
        {
            var saleProducts = context.SaleProducts.ToList();

            if (saleProducts.Count == 0)
            {
                Console.WriteLine("No products in the sale to update.");
                Console.ReadKey();
                return;
            }

            bool updateMoreProducts = true;
            while (updateMoreProducts)
            {
                Console.Clear();
                Console.WriteLine("Products in the current sale:");
                foreach (var saleProduct in saleProducts)
                {
                    Console.WriteLine($"{saleProduct.ProductId}. {saleProduct.ProductName} - {saleProduct.Quantity} @ {saleProduct.ProductPrice:C}");
                }

                Console.Write("Enter the Product ID to update: ");
                int productId = Convert.ToInt32(Console.ReadLine());

                var saleProductToUpdate = saleProducts.FirstOrDefault(p => p.ProductId == productId);
                if (saleProductToUpdate != null)
                {
                    Console.Write("Enter the new quantity: ");
                    int newQuantity = Convert.ToInt32(Console.ReadLine());

                    var originalProduct = context.Products.FirstOrDefault(p => p.Id == productId);
                    if (originalProduct != null)
                    {
                        int originalSaleQuantity = saleProductToUpdate.Quantity;
                        int availableQuantity = originalProduct.quantity + originalSaleQuantity;

                        while (newQuantity < 0 || newQuantity > availableQuantity)
                        {
                            Console.WriteLine("Invalid quantity!");
                            Console.Write("Enter valid quantity: ");
                            newQuantity = Convert.ToInt32(Console.ReadLine());
                        }
                        originalProduct.quantity += originalSaleQuantity; // Restore the original quantity
                        saleProductToUpdate.Quantity = newQuantity;

                        if (newQuantity == 0)
                        {
                            context.SaleProducts.Remove(saleProductToUpdate);
                        }
                        else
                        {
                            originalProduct.quantity -= newQuantity;
                            context.Products.Update(originalProduct);
                        }

                        context.SaveChanges();
                        Console.WriteLine("Product quantity updated in the sale.");
                    }
                    else
                    {
                        Console.WriteLine("Original product not found in inventory.");
                    }
                }
                else
                {
                    Console.WriteLine("Product not found in the current sale.");
                }

                Console.Write("Do you want to update another product in the sale? (y/n): ");
                string? response = Console.ReadLine();
                while (response == null || (response.Trim().ToLower() != "y" && response.Trim().ToLower() != "n"))
                {
                    Console.WriteLine("Invalid response! Enter y or n: ");
                    response = Console.ReadLine();
                }

                updateMoreProducts = response.Trim().ToLower() == "y";
            }
            Console.WriteLine("Sale updated.");
            Generate(context);
        }

        public static void Generate(POSDbContext context)
        {
            Console.Write("Do you want to generate receipt? (y/n): ");
            string? response2 = Console.ReadLine();
            while (response2 == null || (response2.Trim().ToLower() != "y" && response2.Trim().ToLower() != "n"))
            {
                Console.WriteLine("Invalid response! Enter y or n: ");
                response2 = Console.ReadLine();
            }

            if (response2.Trim().ToLower() == "y")
            {
                GenerateReceipt(context);
            }
            else
            {
                Cashier.ShowCashierMenu(context);
            }
        }

        public static void PrintTotalAmount(POSDbContext context)
        {
            Console.WriteLine($"{"Total Amount:",-30} {CalculateTotalAmount(context):C}");
        }

        public static double CalculateTotalAmount(POSDbContext context)
        {
                var total = context.SaleProducts.Sum(s => s.Quantity * s.ProductPrice);
                if (total > 0)
                {
                    return total;
                }
                else
                {
                    Console.WriteLine("Please add products to sale before calculating total amount!");
                    return 0.0;
                }
            
        }


        public static void AddProductToSale(POSDbContext context)
        {
            bool addMoreProducts = true;
            while (addMoreProducts)
            {
                Console.Clear();
                EFInventory.ViewProducts(context);
                Console.Write("Enter Product ID: ");
                int productId = Convert.ToInt32(Console.ReadLine());

                var product = context.Products.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    Console.Write("Now enter the quantity: ");
                    int quantity = Convert.ToInt32(Console.ReadLine());
                    while (quantity <= 0 || quantity > product.quantity)
                    {
                        Console.WriteLine("Invalid quantity!");
                        Console.Write("Enter valid quantity: ");
                        quantity = Convert.ToInt32(Console.ReadLine());
                    }

                    var sale = new SaleProducts
                    {
                        Date = DateTime.Now,
                        Quantity = quantity,
                        ProductId = productId,
                        ProductName = product.name,
                        ProductPrice = product.price
                    };

                    product.quantity -= quantity;
                    context.Products.Update(product);
                    context.SaleProducts.Add(sale);
                    context.SaveChanges();

                    Console.WriteLine("Product added to sale");
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }

                Console.Write("Do you want to add another product to the sale? (y/n): ");
                string? response = Console.ReadLine();
                while (response == null || (response.Trim().ToLower() != "y" && response.Trim().ToLower() != "n"))
                {
                    Console.WriteLine("Invalid response! Enter y or n: ");
                    response = Console.ReadLine();
                }

                addMoreProducts = response.Trim().ToLower() == "y";
            }

            if (context.SaleProducts.Any())
            {
                Console.WriteLine("Sale completed.");
                Generate(context);
            }
            else
            {
                Console.WriteLine("Press any key to go back!");
            }
        }
        public static void GenerateReceipt(POSDbContext context)
        {
            var saleProducts = context.SaleProducts.ToList();

            if (saleProducts.Any())
            {
                Console.WriteLine("Receipt:");
                Console.WriteLine(new string('-', 50));
                Console.WriteLine($"{"Quantity",-10} {"Product",-20} {"Price",-10} {"Total",-10}");
                Console.WriteLine(new string('-', 50));

                foreach (var sale in saleProducts)
                {
                    string totalPrice = (sale.Quantity * sale.ProductPrice).ToString("C");
                    Console.WriteLine($"{sale.Quantity,-10} {sale.ProductName,-20} {sale.ProductPrice,-10:C} {totalPrice,-10}");
                }

                Console.WriteLine(new string('-', 50));
                Console.WriteLine($"{"Total Amount:",-30} {CalculateTotalAmount(context):C}");
                Console.WriteLine(new string('-', 50));

                context.SaleProducts.RemoveRange(saleProducts);
                context.SaveChanges();

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Please add products to sale before generating receipt.");
            }
        }

    }
}
