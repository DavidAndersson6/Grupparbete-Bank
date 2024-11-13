using System;
using System.Collections.Generic;
using System.Timers;

public class TransactionProcessor
{
    private List<Transaction> transactionsList = new List<Transaction>();
    private System.Timers.Timer processTimer;

    public TransactionProcessor()
    {
        processTimer = new System.Timers.Timer(900000); // Timer som triggas var 900 000 millisekunder (15 min)
        processTimer.AutoReset = true; // Automatisk omstart för upprepad körning
        processTimer.Elapsed += ProcessTransactions; // Kör ProcessTransactions var 15:e minut
        processTimer.Start();
    }
    public void AddTransactionFromCustomer() //Metod som låter användarna lägga in en transaktion
    {
        while (true)
        {
            Console.WriteLine("Enter transaction amount: (or 'stop' to stop the transaction)");
            string amountInput = Console.ReadLine();

            if (amountInput == "stop")
            {
                break;
            }
            if (int.TryParse(amountInput, out int amount)) //int.TryParse försöker konvertera strängen amountInput till ett heltal.
            {
                Transaction transaction = new Transaction(amount);
                AddTransaction(transaction);
                Console.WriteLine("The transaction has been added with the amount: " + amount);
            }
            else
            {
                Console.WriteLine("The transaction amount is invalid. Please try again.");
            }
        }
    }

    public void AddTransaction(Transaction transaction) // Metod som lägger till en transaktion i listan
    {
        transactionsList.Add(transaction);
        Console.WriteLine($"Transaction amount: {transaction.Amount}");
    }

    private void ProcessTransactions(object sender, ElapsedEventArgs e) // Metod som körs när timern triggar, var 15:e minut
    {
        Console.WriteLine("Processing transactions...");

        foreach (var transaction in transactionsList)
        {
            Console.WriteLine($"Transaction amount: {transaction.Amount}");
        }
        transactionsList.Clear(); // Rensar listan efter bearbetning
    }

    public class Transaction // Enkel klass för att representera en transaktion
    {
        public int Amount { get; set; }

        public Transaction(int amount)
        {
            Amount = amount;
        }
    }
}