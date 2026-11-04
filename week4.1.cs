using System;
using System.Text.RegularExpressions;

namespace CustomerTransactionManager
{
    struct Customer
    {
        public string Id;
        public string Name;
        public string Phone;
        public int Amount;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Customer[] customers = new Customer[50];
            int customerCount = 0;

            Console.WriteLine("=== Customer Transaction Management ===");

            InputCustomersData(customers, ref customerCount);

            DisplayCustomersAndTotal(customers, customerCount);

            UpdateCustomerAmount(customers, customerCount);
        }

        static void InputCustomersData(Customer[] customers, ref int customerCount)
        {
            while (true)
            {
                if (customerCount >= 50)
                {
                    Console.WriteLine("Warning: Maximum capacity of 50 customers reached. Cannot add more.");
                    break;
                }

                Customer newCustomer;

                while (true)
                {
                    Console.WriteLine("\nEnter customer data (ID,Name,Phone,Amount) e.g., A123456789,Wang Datong,0911234123,680 :");
                    string input = Console.ReadLine();

                    if (ProcessAndValidateInput(input, customers, customerCount, out newCustomer))
                    {
                        break; 
                    }
                }

                customers[customerCount] = newCustomer;
                customerCount++;

                Console.Write("Continue inputting? (1/0) : ");
                string continueInput = Console.ReadLine();
                if (continueInput != "1")
                {
                    break;
                }
            }
        }

        static void DisplayCustomersAndTotal(Customer[] customers, int customerCount)
        {
            Console.WriteLine("\n--- Customer Data List ---");
            int totalAmount = 0;

            for (int i = 0; i < customerCount; i++)
            {
                Console.WriteLine($"{customers[i].Id} {customers[i].Name} {customers[i].Phone} {customers[i].Amount}");
                totalAmount += customers[i].Amount;
            }

            Console.WriteLine($"Total Amount: {totalAmount}");
        }

        static void UpdateCustomerAmount(Customer[] customers, int customerCount)
        {
            while (true)
            {
                Console.Write("\nDo you want to modify transaction amount? (1/0) : ");
                string choice = Console.ReadLine();

                if (choice != "1")
                {
                    break;
                }

                Console.Write("Please enter customer ID to modify: ");
                string searchId = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(searchId))
                {
                    Console.WriteLine("ID cannot be empty.");
                    continue;
                }

                int foundIndex = FindCustomerById(customers, customerCount, searchId.Trim());

                if (foundIndex != -1)
                {
                    int newAmount;
                    while (true)
                    {
                        Console.Write("Please enter new transaction amount: ");
                        string amountInput = Console.ReadLine();

                        if (int.TryParse(amountInput, out newAmount) && newAmount >= 0)
                        {
                            break;
                        }
                        Console.WriteLine("Invalid amount! Amount must be a non-negative whole number (no decimals).");
                    }

                    customers[foundIndex].Amount = newAmount;

                    DisplayCustomersAndTotal(customers, customerCount);
                }
                else
                {
                    Console.WriteLine("Warning: Customer ID not found.");
                }
            }
        }

        
        static bool ProcessAndValidateInput(string input, Customer[] customers, int count, out Customer validCustomer)
        {
            validCustomer = new Customer();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Error: Input cannot be empty or just spaces.");
                return false;
            }

            string[] parts = input.Split(',');

            if (parts.Length != 4)
            {
                Console.WriteLine("Error: Incomplete data. Format must be strictly: ID,Name,Phone,Amount.");
                return false;
            }

            string id = parts[0].Trim();
            string name = parts[1].Trim();
            string phone = parts[2].Trim();
            string amountStr = parts[3].Trim();

            if (!Regex.IsMatch(id, @"^[A-Za-z]\d{9}$"))
            {
                Console.WriteLine("Error: ID must consist of 1 letter followed by 9 digits (total 10 characters).");
                return false;
            }

            if (FindCustomerById(customers, count, id) != -1)
            {
                Console.WriteLine("Error: ID already exists. Duplicates are not allowed.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Error: Name cannot be empty.");
                return false;
            }

            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                Console.WriteLine("Error: Phone must consist of exactly 10 digits without negative signs.");
                return false;
            }

            if (!int.TryParse(amountStr, out int parsedAmount) || parsedAmount < 0)
            {
                Console.WriteLine("Error: Transaction amount must be a non-negative whole number.");
                return false;
            }
            validCustomer.Id = id;
            validCustomer.Name = name;
            validCustomer.Phone = phone;
            validCustomer.Amount = parsedAmount;

            return true;
        }

        static int FindCustomerById(Customer[] customers, int count, string searchId)
        {
            for (int i = 0; i < count; i++)
            {
                if (customers[i].Id.Equals(searchId, StringComparison.OrdinalIgnoreCase))
                {
                    return i; 
                }
            }
            return -1; 
        }
    }
}