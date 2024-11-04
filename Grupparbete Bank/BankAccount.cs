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
    }
}
