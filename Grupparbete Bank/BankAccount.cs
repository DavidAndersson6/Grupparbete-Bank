using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Grupparbete_Bank
{
    public class BankAccount
    {
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public string AccountHolder { get; set; }

        public BankAccount(int accountId, decimal balance, string accountHolder)
        {
            AccountId = accountId;
            Balance = balance;
            AccountHolder = accountHolder;
        }

        public void ListAccounts()
        {
            List<BankAccount> bankAccounts = new List<BankAccount> ();
            bankAccounts.Add(new BankAccount(1, 1000.5m, "Alice"));
            bankAccounts.Add(new BankAccount(2, 600.755m, "Markus"));



            foreach (BankAccount bankAccount in bankAccounts)
            {
                Console.WriteLine($"Account ID: {bankAccount.AccountId}, Balance: {bankAccount.Balance}, Holder: {bankAccount.AccountHolder}");
            }
        }

        public void runBank() 
        {
            bool exit = false;
            BankAccount bankAccount = new BankAccount(0, 0, string.Empty);

            while (!exit)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. List of bank accounts");
                Console.WriteLine("2. Transfer money between two of my own accounts");
                Console.WriteLine("3. Transfer money to other accounts");
                Console.WriteLine("4. Open new account");
                Console.WriteLine("5. Exit");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Listing all bank accounts...");

                        bankAccount.ListAccounts();

                        break;

                    case "2":
                        Console.WriteLine("Transferring money between your own accounts...");
                        // Call the method to transfer between own accounts here
                        break;

                    case "3":
                        Console.WriteLine("Transferring money to other accounts...");
                        // Call the method to transfer to other accounts here
                        break;

                    case "4":
                        Console.WriteLine("Opening a new account...");
                        // Call the method to open a new account here
                        break;

                    case "5":
                        Console.WriteLine("Exiting the program...");
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a number from 1 to 5.");
                        break;
                }

                Console.WriteLine(); // Adds a line for better readability
            }
        }
    }
}
