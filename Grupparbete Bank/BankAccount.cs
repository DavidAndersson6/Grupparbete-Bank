using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Grupparbete_Bank.Transaction;

namespace Grupparbete_Bank
{
    public enum AccountType
    {
        Sparkonto,
        Saldokonto
    }

    public enum Currency
    {
        SEK,
        USD,
        EUR
    }
    public class BankAccount
    {
        public AccountType Type { get; private set; }
        public decimal Balance { get; set; }
        public string AccountNumber { get; set; }
        public Currency AccountCurrency { get; set; } // Currency in which the account operates

        // Constructor to initialize account details
        public BankAccount(string accountNumber, AccountType type, Currency currency, decimal startBalance = 0)
        {
            Type = type;
            Balance = startBalance;
            AccountNumber = accountNumber;
            AccountCurrency = currency;
        }

        // Method to deposit money into the account
        public void Deposit(decimal amount)
        {
            if(amount >= 0)
            {
                Balance += amount;
            }
        }

        // Method to withdraw money from the account, ensuring sufficient balance
        public bool Withdraw(decimal amount)
        {
            if(amount > 0 && amount <= Balance)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }
    }
}
