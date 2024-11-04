using System.Transactions;

namespace Grupparbete_Bank
{
    internal class Program
    {
        static void Main(string[] args)
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
    
