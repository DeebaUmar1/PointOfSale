using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale
{
    public static class Admin
    {
        public static void ShowAdminMenuMain()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Admin Menu");

                Console.WriteLine("1. Product Management");
                Console.WriteLine("2. Inventory Management");
                Console.WriteLine("3. Set User Role");
                Console.WriteLine("4. Log Out");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAdminMenu();
                        break;
                    case "2":
                        InventoryMenu();
                        break;
                    case "3":
                        SetUserRole();
                        break;
                    case "4":
                        return; // Exit the method and the loop
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
                Console.ReadKey();
            }
        }

        public static void ShowAdminMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Product Management");

                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Update Product");
                Console.WriteLine("3. Remove Product");
                Console.WriteLine("4. View Products");
                Console.WriteLine("5. Purchase Products");
                Console.WriteLine("6. Go Back");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        bool done = AddProduct();
                        if (done)
                        {
                            Console.Clear();
                            Console.WriteLine("Product added successfully!");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Product not added!");
                            Console.ReadKey();
                        }
                        break;
                    case "2":
                        Console.Clear();
                        bool updated = Inventory.Update();
                        if (updated)
                        {
                            Console.Clear();
                            Console.WriteLine("Product updated successfully!");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Product not updated!");
                            
                        }
                        break;
                    case "3":
                        Console.Clear();
                        Inventory.RemoveProduct();
                        break;
                    case "4":
                        Inventory.ViewProducts();
                        break;
                    case "5":
                        Console.Clear();
                        Purchase.AddProductToPurchase();
                        break;
                    case "6":
                        return; // Go back to the previous menu
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
                Console.ReadKey();
            }
        }

        public static void InventoryMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Inventory Management");

                Console.WriteLine("1. Receive new stock");
                Console.WriteLine("2. Reduce stock");
                Console.WriteLine("3. Remove products from the stock");
                Console.WriteLine("4. View Products in the stock");
                Console.WriteLine("5. Go Back");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Inventory.UpdateStock("increment");
                        break;
                    case "2":
                        Console.Clear();
                        Inventory.UpdateStock("decrement");
                        break;
                    case "3":
                        Console.Clear();
                        Inventory.RemoveProduct(); ;
                        Console.ReadKey();
                        break;
                    case "4":
                        Inventory.ViewProducts();
                        break;
                    case "5":
                        return; // Go back to the previous menu
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
                Console.ReadKey();
            }
        }

      

        public static bool AddProduct()
        {
            Console.Clear();
            Console.WriteLine("Add product: ");
           
            Console.WriteLine("Enter name of the product: ");
            string? name = Console.ReadLine();

            Console.WriteLine("Enter price of the product: ");
            string? priceStr = Console.ReadLine();

            double price = 0;
             bool valid = false;
                
                while (!valid)
                {
                    if (double.TryParse(priceStr, out price))
                    {
                        valid = true; // Exit the loop if input is a valid integer
                    }
                    else
                    {
                        Console.WriteLine("Invalid price. Please enter a valid number.");
                        Console.WriteLine("Enter new price of the product: ");
                        priceStr = Console.ReadLine();

                    }
                }
            
            Console.WriteLine("Enter quantity of the product: ");
            string? quantityStr = Console.ReadLine();
            int quantity = 0;
            bool validInput = false;
              

                while (!validInput)
                {
                    if (int.TryParse(quantityStr, out quantity))
                    {
                        validInput = true; // Exit the loop if input is a valid integer
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity input. Please enter a valid number.");
                        Console.WriteLine("Enter new quantity of the product: ");
                        quantityStr = Console.ReadLine();
                    }
                
            }
            Console.WriteLine("Enter type of product: ");
            string? type = Console.ReadLine();


            Console.WriteLine("Enter category of product: ");
            string? category = Console.ReadLine();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(priceStr) || string.IsNullOrEmpty(quantityStr) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(category))
            {
                Console.WriteLine("All fields are required.");
                Console.WriteLine("Press any key to add the product again!");
                Console.ReadKey();  
                return AddProduct();
            }
            else
            {
                var prod = new Product { name = name, price = price, quantity = Convert.ToInt32(quantity), type = type, category = category };
                Inventory.Add(prod);
                return true;
            }
           
        }

        public static bool UpdateProduct(int input)
        {
            Console.Clear();
            Console.WriteLine("Update selected product: ");

            Console.WriteLine("Enter new name of the product: ");
            string? name = Console.ReadLine();

            Console.WriteLine("Enter new price of the product: ");
            string? priceStr = Console.ReadLine();

            if (!string.IsNullOrEmpty(priceStr))
            {

                bool valid = false;
                double price = 0;
                while (!valid)
                {
                    if (double.TryParse(priceStr, out price))
                    {
                        valid = true; // Exit the loop if input is a valid integer
                    }
                    else
                    {
                        Console.WriteLine("Invalid price. Please enter a valid number.");
                        Console.WriteLine("Enter new price of the product: ");
                        priceStr = Console.ReadLine();

                    }
                }
            }
            Console.WriteLine("Enter new quantity of the product: ");
            string? quantityStr = Console.ReadLine();
            
            if (!string.IsNullOrEmpty(quantityStr))
            {
                bool validInput = false;
                int quantity = 0;
                while (!validInput)
                {
                    if (int.TryParse(quantityStr, out quantity))
                    {
                        validInput = true; // Exit the loop if input is a valid integer
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity input. Please enter a valid number.");
                        Console.WriteLine("Enter new quantity of the product: ");
                        quantityStr = Console.ReadLine();
                    }
                }
            }
           
            Console.WriteLine("Enter new type of product: ");
            string? type = Console.ReadLine();

          
            Console.WriteLine("Enter new category of product: ");
            string? category = Console.ReadLine();
            
           
           return Inventory.Update2(input, name, category, type, quantityStr , priceStr);
            
        }

        public static void SetUserRole()
        {
            Console.Clear();
            UserData.ViewUsers();
            Console.WriteLine("Set/Update User Role");
            Console.WriteLine("Enter the id of the user: ");
            int id = Convert.ToInt32(Console.ReadLine());

            var user = UserData.Users.Find(x => x.Id == id);
            if (user != null)
            {
                Console.WriteLine("Select the role you want to assign:");
                Console.WriteLine("1. Admin");
                Console.WriteLine("2. Cashier");
                Console.Write("Enter the number corresponding to the role: ");
                int roleSelection;
                while (!int.TryParse(Console.ReadLine(), out roleSelection) || (roleSelection != 1 && roleSelection != 2))
                {
                    Console.WriteLine("Invalid selection. Please enter 1 for Admin or 2 for Cashier.");
                    Console.Write("Enter the number corresponding to the role: ");
                }

                string newRole = roleSelection == 1 ? "Admin" : "Cashier";
                user.role = newRole;
                Console.WriteLine("Role has been updated!");

            }
            else
            {
                Console.WriteLine("No such user exists!");
            }
        }
    }

}
