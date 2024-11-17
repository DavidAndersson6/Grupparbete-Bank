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
        public string SourceAccountCurrency { get; set; }


        public class TransactionLogger
        {
            private static readonly TransactionLogger _instance = new TransactionLogger();
            private List<Transaction> _transactionLog = new List<Transaction>();

            private TransactionLogger() { }

            public static TransactionLogger Instance => _instance;

            // Adds a new transaction entry to the log
            public void AddLogEntry(string transactionType, decimal amount, string sourceAccount, string destinationAccount, string description, string sourceAccountCurrency)
            {
                {
                    var transaction = new Transaction
                    {
                        Timestamp = DateTime.Now,
                        TransactionType = transactionType,
                        Amount = amount,
                        SourceAccount = sourceAccount,
                        DestinationAccount = destinationAccount,
                        Description = description,
                        SourceAccountCurrency = sourceAccountCurrency
                    };
                    _transactionLog.Add(transaction);
                }
            }

            // Formats currency amounts based on the provided currency type
            private static string FormatCurrency(decimal amount, string currency)
            {
                var cultureInfo = currency switch
                {
                    "SEK" => new System.Globalization.CultureInfo("sv-SE"),
                    "USD" => new System.Globalization.CultureInfo("en-US"),
                    "EUR" => new System.Globalization.CultureInfo("de-DE"),
                    _ => System.Globalization.CultureInfo.CurrentCulture // default to system culture
                };

                return amount.ToString("C", cultureInfo);
            }

            // Displays the entire transaction log with formatted details
            public void DisplayLog()
            {
                foreach (var transaction in _transactionLog)
                {
                    string formattedAmount = FormatCurrency(transaction.Amount, transaction.SourceAccountCurrency);
                    Console.WriteLine($"{transaction.Timestamp} - {transaction.TransactionType}: {formattedAmount} from {transaction.SourceAccount} to {transaction.DestinationAccount}. {transaction.Description}");
                }
            }


        }
    }
}