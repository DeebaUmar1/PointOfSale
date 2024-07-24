using AutoMapper.Internal;
using PointOfSale.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Services
{
    public static class EFUserData
    {
        
        public static void SeedData(POSDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Id = 1, name = "admin", email = "email", password = Password.EncodePasswordToBase64("adminpass"), role = "Admin" },
                    new User { Id = 2, name = "cashier", email = "email", password = Password.EncodePasswordToBase64("cashierpass"), role = "Cashier" },
                    new User { Id = 3, name = "manager", email = "email", password = Password.EncodePasswordToBase64("managerpass"), role = "Admin" }
                );
                context.SaveChanges();
            }
        }

        public static void ViewUsers(POSDbContext context)
        {
            Console.Clear();
            Console.WriteLine("User List:");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("ID\tName\t\tEmail\t\tRole");
            Console.WriteLine("------------------------------------------------");

            foreach (var user in context.Users)
            {
                Console.WriteLine($"{user.Id}\t{user.name}\t\t{user.email}\t\t{user.role}");
            }

            Console.WriteLine("------------------------------------------------");
        }

        public static List<User> GetUsers(POSDbContext context)
        {
            return context.Users.ToList();
        }

        public static bool Add(POSDbContext context, User user)
        {
            var users = context.Users.ToList();
            foreach (var item in users)
            {
                if(item.name == user.name || item.email == user.email)
                {
                    return false;
                }
            }
           
                user.Id = context.Users.Max(u => u.Id) + 1;
                context.Users.Add(user);
                context.SaveChanges();
                return true;
            
        }

       
        public static string Search(POSDbContext context, string name, string password)
        {
            var searchResults = context.Users.FirstOrDefault(user => user.name == name);
            if (searchResults == null)
            {
                return null;
            }
            else
            {
                string encryptedPassword = searchResults.password;
                string decryptedPassword = Password.DecodeFrom64(encryptedPassword);

                if (password == decryptedPassword)
                {
                  
                    string? userrole = searchResults.role;
                    if (userrole != null)
                    {

                        return userrole;
                    }
                    else
                    {
                        Console.WriteLine("Role is not assigned yet!");
                        return "norole";
                    }
                   
                }
                else
                {
                    return "Wrong";
                }
            }
        }

    }
}
