using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale
{
    public static class Cashier
    {
        public static void ShowCashierMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Cashier Menu");
                Console.WriteLine("1. Add Products to Sale");
                Console.WriteLine("2. Calculating Total Amount");
                Console.WriteLine("3. Generate Reciept");
                Console.WriteLine("4. Update Products in Sale");
                Console.WriteLine("5. Log out");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        AddProductToSale();
                        break;
                    case "2":
                        Console.Clear();
                        PrintTotalAmount();
                        break;
                    case "3":
                        Console.Clear();
                        GenerateReceipt();
                        break;
                    case "4":
                        Console.Clear();
                        UpdateProductsInSale();
                        break;
                    case "5":
                        return;

                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
                Console.ReadKey();
            }
        }
        public static void UpdateProductsInSale()
        {
            if (Transaction.SaleProducts.Count == 0)
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
                foreach (var saleProduct in Transaction.SaleProducts)
                {
                    Console.WriteLine($"{saleProduct.ProductId}. {saleProduct.ProductName} - {saleProduct.Quantity} @ {saleProduct.ProductPrice:C}");
                }

                Console.Write("Enter the Product ID to update: ");
                var productId = Convert.ToInt32(Console.ReadLine());

                var saleProductToUpdate = Transaction.SaleProducts.Find(p => p.ProductId == productId);
                if (saleProductToUpdate != null)
                {
                    Console.Write("Enter the new quantity: ");
                    var newQuantity = Convert.ToInt32(Console.ReadLine());

                    var originalProduct = Inventory.Products.Find(p => p.Id == productId);
                    if (originalProduct != null)
                    {
                        var originalSaleQuantity = saleProductToUpdate.Quantity;
                        var availableQuantity = originalProduct.quantity + originalSaleQuantity;

                        while (newQuantity < 0 || newQuantity > availableQuantity)
                        {
                            Console.WriteLine("Invalid quantity!");
                            Console.Write("Enter valid quantity: ");
                            newQuantity = Convert.ToInt32(Console.ReadLine());
                        }
                        originalProduct.quantity += saleProductToUpdate.Quantity; // Restore the original quantity
                        saleProductToUpdate.Quantity = newQuantity;
                       
                        if (newQuantity == 0)
                        {
                            Transaction.SaleProducts.Remove(saleProductToUpdate);
                        }
                        else
                        {
                            originalProduct.quantity -= newQuantity;
                        }
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
                while (response == null)
                {
                    Console.WriteLine("You have to enter y or n: ");
                    Console.Write("Do you want to add another product to the sale? (y/n): ");
                    response = Console.ReadLine();
                }
                response = response.Trim().ToLower();
                while (response != "y" && response != "n")
                {
                    Console.WriteLine("Invalid response!");
                    Console.Write("Do you want to add another product to the sale? (y/n): ");
                    response = Console.ReadLine();
                    response = response?.Trim().ToLower();
                }
                if (response != "y")
                {
                    updateMoreProducts = false;
                }
            }
            Console.WriteLine("Sale updated.");
            Generate();
            
        }

        public static void Generate()
        {
            Console.Write("Do you want to generate receipt? (y/n): ");
            string? response2 = Console.ReadLine();
            while (response2 == null)
            {
                Console.WriteLine("You have to enter y or n: ");
                Console.Write("Do you want to generate receipt? (y/n): ");
                response2 = Console.ReadLine();
            }
            response2 = response2.Trim().ToLower();
            while (response2 != "y" && response2 != "n")
            {
                Console.WriteLine("Invalid response!");
                Console.Write("Do you want to generate receipt? (y/n): ");
                response2 = Console.ReadLine();
                response2 = response2?.Trim().ToLower();
            }

            if (response2 == "y")
            {
                GenerateReceipt();
            }
            else
            {
                ShowCashierMenu();
            }
        }
        public static void AddProductToSale()
        {
            bool addMoreProducts = true;
            while (addMoreProducts)
            {
                Console.Clear();
                Inventory.ViewProducts();
                Console.Write("Enter Product ID: ");
                var productId = Convert.ToInt32(Console.ReadLine());

                var product = Inventory.Products.Find(p => p.Id == productId);
                if (product != null)
                {
                    Console.Write("Now enter the quantity: ");
                    var q = Convert.ToInt32(Console.ReadLine());
                    while (q <= 0 || q > product.quantity)
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
                     
                    Transaction.Add(sale);
                    Console.WriteLine("Product added to sale");
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }

                Console.Write("Do you want to add another product to the sale? (y/n): ");
                string? response = Console.ReadLine();
                while(response == null)
                {
                    Console.WriteLine("You have to enter y or n: ");
                    Console.Write("Do you want to add another product to the sale? (y/n): ");
                    response = Console.ReadLine();
                }
                response = response.Trim().ToLower();
                    while (response != "y" && response != "n" )
                    {
                        Console.WriteLine("Invalid response!");
                        Console.Write("Do you want to add another product to the sale? (y/n): ");
                        response = Console.ReadLine();
                        response = response?.Trim().ToLower();
                    }
                if (response != "y")
                {
                    addMoreProducts = false;
                }

            }
            if (Transaction.SaleProducts.Count > 0)
            {
                Console.WriteLine("Sale completed.");
                Generate();
            }
            else
            {
                Console.WriteLine("Press any key to go back!");
            }
         
        }
        public static void PrintTotalAmount()
        {
            Console.WriteLine($"{"Total Amount:",-30} {CalculateTotalAmount():C}");
        }
        public static double CalculateTotalAmount()
        {
            if (Transaction.SaleProducts.Count > 0)
            {

                double total = Transaction.SaleProducts.Sum(s => s.Quantity * s.ProductPrice);
                return total;
            }
            else
            {
                Console.WriteLine("Please add products to sale before calculating total amount!");
                return 0.0;
            }
        }

        public static void GenerateReceipt()
        {
            if (Transaction.SaleProducts.Count > 0)
            {
                Console.WriteLine("Receipt:");
                Console.WriteLine(new string('-', 50));
                Console.WriteLine($"{"Quantity",-10} {"Product",-20} {"Price",-10} {"Total",-10}");
                Console.WriteLine(new string('-', 50));

                foreach (var sale in Transaction.SaleProducts)
                {
                    string totalPrice = (sale.Quantity * sale.ProductPrice).ToString("C");
                    Console.WriteLine($"{sale.Quantity,-10} {sale.ProductName,-20} {sale.ProductPrice,-10:C} {totalPrice,-10}");
                }

                Console.WriteLine(new string('-', 50));
                Console.WriteLine($"{"Total Amount:",-30} {CalculateTotalAmount():C}");
                Console.WriteLine(new string('-', 50));

                Transaction.SaleProducts.Clear();
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
