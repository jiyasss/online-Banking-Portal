using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace TEST_01
{
    class Validation
    {
        public class EmailValidator
        {
            public static string Email(string email)
            {
                bool result = false;
                while (!result)
                {
                    string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                    Regex regex = new Regex(emailPattern);
                    result = regex.IsMatch(email);

                    if (!result)
                    {
                        Console.WriteLine("Please enter a valid email address:");
                        email = Console.ReadLine();
                    }
                }
                return email;
            }
        }

        public class PasswordValidator
        {
            public static string ValidatePassword(string pass)
            {
                bool result = false;
                while (!result)
                {
                    string passPattern = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).{8,15}$";
                    Regex regex = new Regex(passPattern);
                    result = regex.IsMatch(pass);

                    if (!result)
                    {
                        Console.WriteLine("create unique and different password.");
                        Console.Write("Please enter a valid password: ");
                        pass = Console.ReadLine();
                    }
                }
                return pass;
            }
        }

        public class StringValidator
        {
            public static string ValidateString(string input)
            {
                StringBuilder modifiedStr = new StringBuilder();
                bool validInput = false;

                while (!validInput)
                {
                    foreach (char c in input)
                    {
                        if (char.IsLetter(c))
                        {
                            modifiedStr.Append(c);
                        }
                    }

                    if (modifiedStr.Length > 0)
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter alphabetic characters only:");
                        modifiedStr.Clear();
                        input = Console.ReadLine();
                    }
                }
                return modifiedStr.ToString();
            }
        }

        public static string ValidatePhoneNumber(string phoneNumber)
        {
            bool result = false;
            while (!result)
            {
                string phonePattern = @"^\d{10}$";
                Regex regex = new Regex(phonePattern);
                result = regex.IsMatch(phoneNumber);

                if (!result)
                {
                    Console.WriteLine("Phone number must beof 10 digits.");
                    Console.Write("Please enter a valid phone number: ");
                    phoneNumber = Console.ReadLine();
                }
            }
            return phoneNumber;
        }
    }

    class Registration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }

        public static string directoryPath;
        
        public static string errorFilePath;
        public override string ToString()
        {
            return $"{Username},{Password},{FirstName},{LastName},{Email},{PhoneNumber}";
        }

        static Registration()
        {
            Declare();
        }

        public static void Declare()
        {
            string pathTo = Assembly.GetExecutingAssembly().Location;
            DirectoryInfo Dinfo = new DirectoryInfo(pathTo).Parent.Parent.Parent;
            directoryPath = Path.Combine(Dinfo.FullName, "output");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
          
            errorFilePath = Path.Combine(directoryPath, "error.txt");
        }

        public Registration(string firstname, string lastname, string email, string password, string phoneNumber, string username)
        {
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            Username = username;
        }

    }

    class Account
    {
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public double Balance { get; set; }
        public  Account(string accountNumber , string ifsc , double balance)
        {
            AccountNumber = accountNumber;
            IFSCCode = ifsc;
            Balance = balance;
        }
        
    }

    class System
    {
        static List<Registration> registrations = new List<Registration>();
            static List<Account> accounts = new List<Account>();

        static void DisplayMenu()
        {
            while (true)
            {
                Console.WriteLine("WELCOME TO RBL BANK PORTAL");
                Console.WriteLine("PRESS 1 : REGISTRATION");
                Console.WriteLine("PRESS 2: CREATE ACCOUNT");
                Console.WriteLine("PRESS 3. VIEW ACCOUNT");
                Console.WriteLine("PRESS 4 : LIST OF ALL USERS");
                Console.WriteLine("PRESS 5 : EXIT");
                Console.WriteLine("\n--------------\n");
                Console.WriteLine("KINDLY CHOOSE ONE OPTION FROM ABOVE MENU");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterUser();
                        break;
                    case "2":
                        CreateAccount();
                        break;
                    case "3":
                        ViewAccounts();
                        break;
                    case "4":
                        ListUsers();
                        break;
                    case "5":
                        Console.WriteLine("THANK YOU FOR VISITING OUR PORTAL :)");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("INVALID CHOICE, PLEASE ENTER AGAIN CORRECTLY");
                        break;
                }
            }
        }

        static void RegisterUser()
        {
            Console.WriteLine("PLEASE ENTER YOUR FIRST NAME:");
            string firstname = Validation.StringValidator.ValidateString(Console.ReadLine());
            Console.WriteLine("PLEASE ENTER YOUR LAST NAME:");
            string lastname = Validation.StringValidator.ValidateString(Console.ReadLine());
            Console.WriteLine("PLEASE ENTER YOUR EMAIL:");
            string email = Validation.EmailValidator.Email(Console.ReadLine());
            Console.WriteLine("PLEASE ENTER PHONE NUMBER:");
            string phonenumber = Validation.ValidatePhoneNumber(Console.ReadLine());
            Console.WriteLine("CREATE A USERNAME:");
            string username = Console.ReadLine();
            Console.WriteLine("PLEASE ENTER YOUR PASSWORD:");
            string password = Validation.PasswordValidator.ValidatePassword(Console.ReadLine());

            if (ValidateRegistration(firstname, lastname, email, phonenumber, username, password))
            {
                registrations.Add(new Registration(firstname, lastname, email, password, phonenumber, username));
                Console.WriteLine("USER REGISTERED SUCCESSFULLY!");

                SaveRegistrationDetails();
            }
            else
            {
                Console.WriteLine("REGISTRATION FAILED!");
            }

        }
        static void SaveRegistrationDetails()
        {
            string filePath = Path.Combine(Registration.directoryPath, "registrations.txt");
            string errorFilePath = Path.Combine(Registration.directoryPath, "log_error.txt");

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    foreach (var user in registrations)
                    {
                        writer.WriteLine($"First Name: {user.FirstName}");
                        writer.WriteLine($"Last Name: {user.LastName}");
                        writer.WriteLine($"Email: {user.Email}");
                        writer.WriteLine($"Phone Number: {user.PhoneNumber}");
                        writer.WriteLine($"Username: {user.Username}");
                        writer.WriteLine("------------------------------------");
                    }
                }

                Console.WriteLine("Registration details SAVED to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error !! {ex.Message}");

                // Save error to file
                SaveErrorToFile(errorFilePath, ex);
            }
        }

        static void SaveErrorToFile(string filePath, Exception e)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    Console.WriteLine($"Error: {e.Message}");
                    Console.WriteLine("------------------------------------");
                }

                Console.WriteLine("Error SAVED to log_error.txt.");
            }
            catch (Exception saveEx)
            {
                Console.WriteLine($"Error !! {saveEx.Message}");
            }
            

        }


        static bool ValidateRegistration(string firstName, string lastName, string email, string phoneNo, string username, string password)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phoneNo) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("ALL FIELDS ARE REQUIRED");
                return false;
            }

            foreach (var user in registrations)
            {
                if (user.Username == username)
                {
                    Console.WriteLine("USERNAME ALREADY EXISTS");
                    return false;
                }
            }

            return true;
        }

        static void CreateAccount()
        {
            Console.WriteLine("PLEASE ENTER ACCOUNT NUMBER:");
            string accountNo = Console.ReadLine();
            Console.WriteLine("PLEASE ENTER IFSC CODE:");
            string ifsc = Console.ReadLine();
            Console.WriteLine("PLEASE ADD ACCOUNT BALANCE:");
            double balance;
            while (!double.TryParse(Console.ReadLine(), out balance))
            {
                Console.WriteLine("INVALID INPUT. PLEASE ENTER A VALID NUMBER.");
            }

            if (ValidateAccount(accountNo, ifsc, balance))
            {
                accounts.Add(new Account(accountNo, ifsc, balance));
                Console.WriteLine("ACCOUNT ADDED SUCCESSFULLY!");

            }
            else
            {
                Console.WriteLine("ACCOUNT CREATION FAILED!");
            }


        }
        static bool ValidateAccount(string accountNo, string ifsc, double balance)
        {
            if (string.IsNullOrEmpty(accountNo) || string.IsNullOrEmpty(ifsc))
            {
                Console.WriteLine("ACCOUNT NUMBER AND IFSC CODE ARE REQUIRED");
                return false;
            }

            if (balance < 0)
            {
                Console.WriteLine("BALANCE CANNOT BE NEGATIVE");
                return false;
            }

            return true;
        }

        static void ViewAccounts()
        {
            Console.WriteLine("TOTAL ADDED ACCOUNTS: " + accounts.Count);
            foreach (var account in accounts)
            {
                Console.WriteLine($"ACCOUNT NO.: {account.AccountNumber}");
                Console.WriteLine($"IFSC CODE: {account.IFSCCode}");
                Console.WriteLine($"BALANCE: {account.Balance}");
                Console.WriteLine();
            }
        }


        static void ListUsers()
        {
            Console.WriteLine("TOTAL USERS ADDED: " + registrations.Count);
            foreach (var user in registrations)
            {
                Console.WriteLine($"USERNAME: {user.Username}");
            }
        }

        static void Main(string[] args)
        {
            DisplayMenu();
        }
    }
}
