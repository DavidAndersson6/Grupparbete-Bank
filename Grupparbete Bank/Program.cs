using System.Transactions;
using static Grupparbete_Bank.Transaction;

namespace Grupparbete_Bank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BankApp app = new BankApp();
            app.Run();
        }
    }
}
    
