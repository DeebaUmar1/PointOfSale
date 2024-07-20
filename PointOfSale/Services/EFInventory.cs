using Microsoft.Extensions.DependencyInjection;
using PointOfSale.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Services
{
    public static class EFInventory
    {
        public static void SeedProducts(POSDbContext context)
        {
            // Check if the database contains any products
            if (!context.Products.Any())
            {
                // Add initial product data
                context.Products.AddRange(
                    new Product
                    {
                        Id = 1,
                        name = "Laptop",
                        price = 899.99,
                        quantity = 10,
                        type = "Electronics",
                        category = "Computers"
                    },
                    new Product
                    {
                        Id = 2,
                        name = "Mouse",
                        price = 29.99,
                        quantity = 50,
                        type = "Peripherals",
                        category = "Accessories"
                    },
                    new Product
                    {
                        Id = 3,
                        name = "Keyboard",
                        price = 49.99,
                        quantity = 25,
                        type = "Peripherals",
                        category = "Accessories"
                    }
                );

                // Save changes to the database
                context.SaveChanges();
            }
        }
        public static void Add(POSDbContext context, Product product)
        {
            SeedProducts(context);
            product.Id = context.Products.Max(u => u.Id) + 1;
            context.Products.Add(product);
            context.SaveChanges();
        }

        public static void ViewProducts(POSDbContext context)
        {
          
            var products = context.Products.ToList();
            if (products.Count == 0)
            {
                Console.WriteLine("No products available.");
            }
            else
            {
                Console.WriteLine("Products List:");
                Console.WriteLine(new string('-', 80));
                Console.WriteLine($"{"ID",-5} {"Name",-20} {"Price",-10} {"Quantity",-10} {"Type",-15} {"Category",-15}");
                Console.WriteLine(new string('-', 80));

                foreach (var product in products)
                {
                    if (product.quantity != 0)
                    {
                        Console.WriteLine($"{product.Id,-5} {product.name,-20} {product.price,-10:C} {product.quantity,-10} {product.type,-15} {product.category,-15}");
                    }
                }

                Console.WriteLine(new string('-', 80));
            }
            
        }

        public static void RemoveProduct(POSDbContext context)
        {
            Console.Clear();
            var products = context.Products.ToList();
            if (products.Count == 0)
            {
                Console.WriteLine("No products to remove.");
                Console.ReadKey();
                return;
            }

            ViewProducts(context);
            Console.Write("Enter the id of the product to remove: ");
            string? input = Console.ReadLine();
            int productNumber = Convert.ToInt32(input);

            if (productNumber > 0)
            {
                var prod = context.Products.FirstOrDefault(p => p.Id == productNumber);
                if (prod != null)
                {

                    context.Products.Remove(prod);
                    context.SaveChanges();
                    Console.WriteLine("Product removed successfully!");
                }
                else
                {
                    Console.WriteLine("Product does not exists");
                }
                //Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Invalid number! Please try again.");
            }
            //Console.ReadKey();
        }

        public static bool Update(POSDbContext context)
        {
            Console.Clear();
            ViewProducts(context);
            Console.WriteLine("Enter the id of the product you want to update: ");
            string? query = Console.ReadLine();
            if (query != null)
            {
                if (int.TryParse(query, out int input))
                {
                   var product = context.Products.Find(input);
                    if (product != null)
                    {
                        // Update product logic (you might want to extract this into a separate method)
                        var updated = Admin.UpdateProduct(context, product.Id);
                        if (updated)
                        {
                            context.SaveChanges();
                            return true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No matching products found.");
                    }
                    
                }
            }
            return false;
        }

        public static bool Update2(POSDbContext context, int id, string? name, string? category, string? type, string? quantity, string? price)
        {
            try
            {
                
                var product = context.Products.Find(id);
                if (product != null)
                {
                    product.name = !string.IsNullOrEmpty(name) ? name : product.name;
                    product.category = !string.IsNullOrEmpty(category) ? category : product.category;
                    product.type = !string.IsNullOrEmpty(type) ? type : product.type;
                    product.quantity = !string.IsNullOrEmpty(quantity) ? int.Parse(quantity) : product.quantity;
                    product.price = !string.IsNullOrEmpty(price) ? double.Parse(price) : product.price;

                    context.SaveChanges();
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return false;
        }


        public static void UpdateStock(POSDbContext context, string option)
        {

            Console.Clear();
            ViewProducts(context);
            Console.WriteLine("Enter the id of the product you want to update: ");
            string? query = Console.ReadLine();
            if (query != null)
            {
                int input = Convert.ToInt32(query);
                var products = context.Products.ToList();
                var searchResults = products.Find(prod => prod.Id.Equals(input));
                if (searchResults is null)
                {
                    Console.WriteLine("No matching products found.");

                }
                else
                {
                    if (option == "increment")
                    {
                        Console.WriteLine("Enter quantity to add: ");
                    }
                    else
                    {
                        Console.WriteLine("Enter quantity to remove: ");
                    }
                    string? input2 = Console.ReadLine();

                    while (true)
                    {
                        if (string.IsNullOrEmpty(input2))
                        {
                            Console.WriteLine("Retaining the original quantity!");

                        }
                        else
                        {
                            bool isNumeric = int.TryParse(input2, out int quantity);

                            if (isNumeric && quantity > 0)
                            {
                                if (option == "increment")
                                {
                                    searchResults.quantity += quantity;
                                }
                                else
                                {
                                    searchResults.quantity -= quantity;
                                }
                                Console.WriteLine($"The updated quantity is: {searchResults.quantity}");
                                break;
                            }
                            else if (quantity == 0)
                            {
                               // products.Remove(searchResults);
                                context.Products.Remove(searchResults);
                                context.SaveChanges();
                                Console.WriteLine("Product has been removed");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid quantity! Please enter a non-negative numeric value.");

                            }
                        }

                        // Prompt the user to enter a valid quantity again
                        Console.Write("Enter valid quantity: ");
                        input2 = Console.ReadLine();
                    }

                }
            }
            else
            {
                Console.WriteLine("Invalid! Enter a valid ID: ");
                while (query == null)
                {
                    Console.WriteLine("Enter the id of the product you want to update: ");
                    query = Console.ReadLine();
                }
            }
        }
    }
}

