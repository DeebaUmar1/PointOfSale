﻿using log4net.Util;
using PointOfSale.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PointOfSale.Services
{
    public static class Authentication
    {


        public static void Login(POSDbContext context)
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
                Login(context);
            }
            else
            {
                string role;
                if (context == null)
                {
                    role = UserData.Search(name, password);
                }
                else
                {
                    role = EFUserData.Search(context, name, password);
                }
                if (role == null)
                {
                    Console.WriteLine("No such user exists! Please register before logging in!");
                    Register(context);
                }
                else
                {
                    if (role == "Admin")
                    {
                        Console.WriteLine("You have successfully logged in!");
                        Admin.ShowAdminMenuMain(context);
                    }
                    else if (role == "Cashier")
                    {
                        Console.WriteLine("You have successfully logged in!");
                        Cashier.ShowCashierMenu(context);

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
                        Login(context);

                    }
                    else
                    {
                        Console.WriteLine("No such user exists. Please register yourself before logging in.");
                        // Console.ReadLine();

                    }
                }
            }
        }

        public static void Register(POSDbContext context)
        {
            Console.WriteLine("Registration: ");
            Console.WriteLine("Enter your name: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("All fields are required. Register again");
                Register(context);
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
                    bool added = false;
                    if (context == null)
                    {
                        added = UserData.Add(user);

                    }
                    else
                    {
                        added = EFUserData.Add(context, user);

                    }
                    if (added)
                    {


                        Console.WriteLine("User registered successfully with default role 'Cashier'.\"");
                        Console.WriteLine("Press any key to proceed..");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("User already exists!");
                        Console.WriteLine("Register again!");
                        Console.ReadKey();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.ReadLine();
                }
            }
        }
        
        public static bool LoginAPI(POSDbContext context, string username, string password)
        { 
        
            string role = EFUserData.Search(context, username, password);
            if (role  == "Wrong")
            {  
                    return false;
            }
            else
            {
                 return true;
            } 
       
        }

        public static bool RegisterAPI(POSDbContext context, User user)
        {
            string pswd = Password.EncodePasswordToBase64(user.password);
            user.password = pswd;


            return EFUserData.Add(context, user); ;
        }
    }
}
