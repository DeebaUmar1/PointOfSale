using System;
using System.Globalization;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PointOfSale.Services;

namespace PointOfSale
{
    class Program
    {   
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<POSDbContext>(options => options.UseInMemoryDatabase("POSDatabase"))
                .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<POSDbContext>();

                // Ensure the database is created
                context.Database.EnsureCreated();
                EFUserData.SeedData(context);
                EFInventory.SeedProducts(context);
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Demo POS Application");
                    Console.WriteLine("1. Static List Entities");
                    Console.WriteLine("2. Entity Framework");
                    Console.WriteLine("3. Exit");
                    Console.Write("Enter your choice: ");

                    string? choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            RunApplication(null);
                            break;
                        case "2":
                            RunApplication(context);
                            break;
                        case "3":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice! Please try again.");
                            break;
                    }
                }
                
               // RunApplication(context);
            }

        }
        static void RunApplication(POSDbContext? context)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Demo POS Application");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Authentication.Register(context);
                        break;
                    case "2":
                        Authentication.Login(context);
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
            }
        }

    }
}