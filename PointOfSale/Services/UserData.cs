using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointOfSale.Data;

namespace PointOfSale.Services
{
    public static class UserData
    {
        public static List<User> Users = new List<User>()
             {
                new User { Id = 1, name = "admin", email = "email", password = Password.EncodePasswordToBase64("adminpass"), role = "Admin" },
                new User { Id = 2, name = "cashier", email = "email", password = Password.EncodePasswordToBase64("cashierpass"), role = "Cashier" },
                new User { Id = 3, name = "manager", email = "email", password = "managerpass", role = "Admin" },
                // Add more users as needed
            };
        private static int _userIdCounter = 4;

        public static void ViewUsers()
        {
            Console.Clear();
            Console.WriteLine("User List:");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("ID\tName\t\tEmail\t\tRole");
            Console.WriteLine("------------------------------------------------");

            foreach (var user in Users)
            {
                Console.WriteLine($"{user.Id}\t{user.name}\t\t{user.email}\t\t{user.role}");
            }

            Console.WriteLine("------------------------------------------------");

        }
        public static List<User> GetUsers()
        {
            return Users;
        }

        public static bool Add(User user)
        {

           
            foreach (var item in Users)
            {
                if (item.name == user.name || item.email == user.email)
                {
                    return false;
                }
            }

            user.Id = _userIdCounter++;
            Users.Add(user);
            return true;
        }
        public static string Search(string name, string password)
        {
            //Console.WriteLine(name, password);
            var searchResults = Users.FirstOrDefault(user => user.name == name);
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
