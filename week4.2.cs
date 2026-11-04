using System;
using System.Text.RegularExpressions;

namespace CustomerTierSystem
{
    class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int Amount { get; set; }

        public string GetCustomerTier()
        {
            if (Amount <= 50000)
            {
                return "Regular Customer"; 
            }
            else if (Amount <= 100000)
            {
                return "VIP"; 
            }
            else
            {
                return "VVIP"; 
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Customer[] customers = new Customer[50];
            int customerCount = 0;

            Console.WriteLine("=== Customer Tier Management System ===");

            while (true)
            {
                if (customerCount >= 50)
                {
                    Console.WriteLine("\nWarning: Maximum capacity of 50 customers reached. Cannot add more data.");
                    break;
                }

                Console.WriteLine("\n--- Add New Customer ---");
                AddNewCustomer(customers, ref customerCount);

                Console.Write("\nDo you want to input another customer? (1 for Yes / 0 for No): ");
                string choice = Console.ReadLine();
                if (choice != "1")
                {
                    break;
                }
            }

            Console.WriteLine("Program exited. Thank you.");
        }

        static void AddNewCustomer(Customer[] customers, ref int count)
        {
            Customer newCustomer = new Customer();

            newCustomer.Id = GetValidId(customers, count);
            newCustomer.Name = GetValidName();
            newCustomer.Phone = GetValidPhone();
            newCustomer.Amount = GetValidAmount();

            customers[count] = newCustomer;

            Console.WriteLine($"\n{newCustomer.Name}'s customer tier is: {newCustomer.GetCustomerTier()}");

            count++;
        }


        static string GetValidId(Customer[] customers, int count)
        {
            while (true)
            {
                Console.Write("Please enter ID number : ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Error: ID cannot be empty or just spaces. Please try again.");
                    continue;
                }

                string id = input.Trim();

                if (!Regex.IsMatch(id, @"^[A-Za-z]\d{9}$"))
                {
                    Console.WriteLine("Error: ID must consist of exactly 1 letter followed by 9 digits. Please try again.");
                    continue;
                }

                if (IsIdDuplicate(customers, count, id))
                {
                    Console.WriteLine($"Error: ID '{id}' already exists in the system. Please enter a different unique ID.");
                    continue;
                }

                return id;
            }
        }

        static string GetValidName()
        {
            while (true)
            {
                Console.Write("Please enter Name : ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Error: Name cannot be empty or just spaces. Please try again.");
                    continue;
                }

                return input.Trim();
            }
        }

        static string GetValidPhone()
        {
            while (true)
            {
                Console.Write("Please enter Phone : ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Error: Phone cannot be empty. Please try again.");
                    continue;
                }

                string phone = input.Trim();

                if (!Regex.IsMatch(phone, @"^\d{10}$"))
                {
                    Console.WriteLine("Error: Phone number must consist of exactly 10 digits without any negative signs or characters. Please try again.");
                    continue;
                }

                return phone;
            }
        }

        static int GetValidAmount()
        {
            while (true)
            {
                Console.Write("Please enter Amount : ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Error: Amount cannot be empty. Please try again.");
                    continue;
                }

                if (int.TryParse(input.Trim(), out int amount) && amount >= 0)
                {
                    return amount;
                }

                Console.WriteLine("Error: Transaction amount must be a valid, non-negative whole number (no decimals). Please try again.");
            }
        }

        static bool IsIdDuplicate(Customer[] customers, int count, string searchId)
        {
            for (int i = 0; i < count; i++)
            {
                if (customers[i].Id.Equals(searchId, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
