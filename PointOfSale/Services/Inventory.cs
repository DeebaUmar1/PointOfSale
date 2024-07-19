using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointOfSale.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PointOfSale.Services
{
    public static class Inventory
    {
        public static List<Product> Products = new List<Product>()
        {
            new Product { Id = 1, name = "Laptop", price = 899.99, quantity = 10, type = "Electronics", category = "Computers" },
            new Product { Id = 2, name = "Mouse", price = 29.99, quantity = 50, type = "Peripherals", category = "Accessories" },
        };

        private static int _productIdCounter = 3;

        public static void Add(Product product)
        {
            product.Id = _productIdCounter++;
            Products.Add(product);
        }

        public static void ViewProducts()
        {
            Console.Clear();
            if (Products.Count == 0)
            {
                Console.WriteLine("No products available.");
            }
            else
            {
                Console.WriteLine("Products List:");
                Console.WriteLine(new string('-', 80));
                Console.WriteLine($"{"ID",-5} {"Name",-20} {"Price",-10} {"Quantity",-10} {"Type",-15} {"Category",-15}");
                Console.WriteLine(new string('-', 80));

                foreach (var product in Products)
                {
                    if (product.quantity != 0)
                    {
                        Console.WriteLine($"{product.Id,-5} {product.name,-20} {product.price,-10:C} {product.quantity,-10} {product.type,-15} {product.category,-15}");
                    }
                }

                Console.WriteLine(new string('-', 80));
            }


        }


        public static bool Update()
        {
            bool updated = false;
            Console.Clear();
            ViewProducts();
            Console.WriteLine("Enter the id of the product you want to update: ");
            string? query = Console.ReadLine();
            if (query != null)
            {
                int input = Convert.ToInt32(query);

                var searchResults = Products.Find(prod => prod.Id.Equals(input));
                if (searchResults is null)
                {
                    Console.WriteLine("No matching products found.");
                    return false;
                }
                else
                {

                    updated = Admin.UpdateProduct(null, input);

                }
            }
            if (updated)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public static bool Update2(int id, string? name, string? category, string? type, string? quantity, string? price)
        {
            // Find the product by ID
            try
            {
                var product = Products.Find(p => p.Id == id);
                if (product != null)
                {
                    product.name = !string.IsNullOrEmpty(name) ? name : product.name;
                    product.category = !string.IsNullOrEmpty(category) ? category : product.category;
                    product.type = !string.IsNullOrEmpty(type) ? type : product.type;
                    product.quantity = !string.IsNullOrEmpty(quantity) ? int.Parse(quantity) : product.quantity;
                    product.price = !string.IsNullOrEmpty(price) ? double.Parse(price) : product.price;

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Optionally log the exception or take other actions
                return false;
            }
            return false;
        }


        public static void RemoveProduct()
        {
            Console.Clear();
            if (Products.Count == 0)
            {
                Console.WriteLine("No products to remove.");
                Console.ReadKey();
                return;
            }

            ViewProducts();
            Console.Write("Enter the id of the product to remove: ");
            string? input = Console.ReadLine();
            int productNumber = Convert.ToInt32(input);

            if (productNumber > 0)
            {
                Products.RemoveAt(productNumber - 1);
                Console.WriteLine("Product removed successfully!");
                //Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Invalid number! Please try again.");
            }
            //Console.ReadKey();
        }

        public static void UpdateStock(string option)
        {

            Console.Clear();
            ViewProducts();
            Console.WriteLine("Enter the id of the product you want to update: ");
            string? query = Console.ReadLine();
            if (query != null)
            {
                int input = Convert.ToInt32(query);

                var searchResults = Products.Find(prod => prod.Id.Equals(input));
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
                                Products.Remove(searchResults);
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
