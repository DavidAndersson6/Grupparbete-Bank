using System.Net.Sockets;

class Program
{
    static void Main(string[] args)
    {
        var processor = new TransactionProcessor();
        
        processor.AddTransactionFromCustomer();
        
        Console.WriteLine("All the transactions have been processed. Waiting for processing every 15 minutes..");
        Console.ReadLine();
    }
}    