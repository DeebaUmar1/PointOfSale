using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PointOfSale
{
    public static class Authentication
    {
        public static void Login()
        {
            //Console.Clear();
            Console.WriteLine("Please login: ");
            Console.Write("Enter your name: ");
            string? name = Console.ReadLine();

            Console.Write("Enter your password: ");
            string? password = Console.ReadLine();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("All fields are required.");
                Login();
            }
            else
            {
                string role = UserData.Search(name, password);
                if (role == null)
                {
                    Console.WriteLine("No such user exists! Please register before logging in!");
                    Register();
                }
                else
                {
                    if (role == "Admin")
                    {
                        Console.WriteLine("You have successfully logged in!");
                        Admin.ShowAdminMenuMain();
                    }
                    else if (role == "Cashier")
                    {
                        Console.WriteLine("You have successfully logged in!");
                        Cashier.ShowCashierMenu();

                    }
                    else if (role == "Wrong")
                    {
                        Console.WriteLine("Wrong Password!");
                        
                    }
                    else if (role == "norole")
                    {

                        Console.WriteLine("Admin has not assigned your role yet! \n Admin! please assign role!");
                        Console.WriteLine("Press any key to go to login!");
                        Console.ReadKey();
                        Login();

                    }
                    else
                    {
                        Console.WriteLine("No such user exists. Please register yourself before logging in.");
                        // Console.ReadLine();

                    }
                }
            }
        }

        public static void Register()
        {
            Console.WriteLine("Registration: ");
            Console.WriteLine("Enter your name: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("All fields are required. Register again");
                Register();
            }
            else
            {
                Console.WriteLine("Enter your email: ");
                string? email = Console.ReadLine();
                bool isValid = EmailValidation.IsValidEmail(email);
                while (!isValid)
                {
                    Console.WriteLine("Invalid Email! Register Again");
                    Console.WriteLine("Enter your email again: ");
                    email = Console.ReadLine();
                    if (email != null)
                    {

                        isValid = EmailValidation.IsValidEmail(email);
                    }
                    else
                    {
                        isValid = false;
                    }
                }
                Console.WriteLine("Enter your password: ");
                string? pswd2 = Console.ReadLine();
                Regex vaildate_password = Password.ValidatePassword();
                while (vaildate_password.IsMatch(pswd2) != true)
                {

                    Console.WriteLine("Invalid Password! Password must be atleast 8 to 15 characters. " +
                        "It contains atleast one Upper case and numbers.");
                    Console.WriteLine("Enter your password again: ");
                    pswd2 = Console.ReadLine();
                }

                string pswd = Password.EncodePasswordToBase64(pswd2);
#pragma warning disable CS8601 // Possible null reference assignment.
                var user = new User { email = email, password = pswd, name = name, role = "Cashier" };
#pragma warning restore CS8601 // Possible null reference assignment.
                try
                {
                    UserData.Add(user);
                    Console.WriteLine("User registered successfully with default role 'Cashier'.\"");
                    Console.WriteLine("Press any key to proceed..");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.ReadLine();
                }
            }
        } 
    }
}
