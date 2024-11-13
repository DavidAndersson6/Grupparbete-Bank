using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grupparbete_Bank.Transaction;

namespace Grupparbete_Bank
{

    public class LogViewer
    {
        public static void ShowLog()
        {
            // Display log entries
            Console.WriteLine("Transaction Log:");
            TransactionLogger.Instance.DisplayLog();
        }
    }

}