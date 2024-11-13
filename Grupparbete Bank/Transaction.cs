using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupparbete_Bank
{
    public class Transaction
    {
        public DateTime Timestamp { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string SourceAccount { get; set; }
        public string DestinationAccount { get; set; }
        public string Description { get; set; }


        public class TransactionLogger
        {
            private static readonly TransactionLogger _instance = new TransactionLogger();
            private List<Transaction> _transactionLog = new List<Transaction>();

            private TransactionLogger() { }

            public static TransactionLogger Instance => _instance;

            public void AddLogEntry(string transactionType, decimal amount, string sourceAccount, string destinationAccount, string description)
            {
                {
                    var transaction = new Transaction
                    {
                        Timestamp = DateTime.Now,
                        TransactionType = transactionType,
                        Amount = amount,
                        SourceAccount = sourceAccount,
                        DestinationAccount = destinationAccount,
                        Description = description
                    };
                    _transactionLog.Add(transaction);
                }
            }
            public void DisplayLog()
            {

                foreach (var transaction in _transactionLog)
                {
                    Console.WriteLine($"{transaction.Timestamp} - {transaction.TransactionType}: {transaction.Amount} from {transaction.SourceAccount} to {transaction.DestinationAccount}. {transaction.Description}");
                }
            }


        }
    }
}