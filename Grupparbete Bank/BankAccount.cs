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

        public static List<BankAccount> bankAccounts;
      
        public BankAccount(int accountId, decimal balance, string accountHolder)
        {
            AccountId = accountId;
            Balance = balance;
            AccountHolder = accountHolder;
        }


        public static void InitializeAccounts()
        {
            bankAccounts = new List<BankAccount>
            {
                new BankAccount(1, 1000.5m, "Alice"),
                new BankAccount(2, 600.755m, "Markus")
            };
        }


        public static void ListAccounts()
        {
          
            foreach (BankAccount bankAccount in bankAccounts)
            {
                Console.WriteLine($"Account ID: {bankAccount.AccountId}, Balance: {bankAccount.Balance}, Holder: {bankAccount.AccountHolder}");
            }
        }


        //int sourceAccountId, int targetAccountId, decimal amount
        public static void TransferInternalAccount()
        {
            Console.WriteLine("Write Account ID of the account you want to withdraw from: ");
            if (int.TryParse(Console.ReadLine(), out int sourceAccountId))
            {
                // Find the source account in the list
                BankAccount sourceAccount = bankAccounts.FirstOrDefault(acc => acc.AccountId == sourceAccountId);

                if (sourceAccount != null)
                {
                    Console.WriteLine("Write Account ID of the account you want to transfer to: ");
                    if (int.TryParse(Console.ReadLine(), out int targetAccountId))
                    {
                        // Find the target account in the list
                        BankAccount targetAccount = bankAccounts.FirstOrDefault(acc => acc.AccountId == targetAccountId);

                        if (targetAccount != null)
                        {
                            Console.WriteLine("Enter amount to transfer: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                            {
                                if (sourceAccount.Balance >= amount)
                                {
                                    // Perform the transfer
                                    sourceAccount.Balance -= amount;
                                    targetAccount.Balance += amount;
                                    Console.WriteLine("Transfer successful!");
                                }
                                else
                                {
                                    Console.WriteLine("Insufficient funds.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount entered.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Target account not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid target account ID.");
                    }
                }
                else
                {
                    Console.WriteLine("Source account not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid source account ID.");
            }
        }

    }
}
