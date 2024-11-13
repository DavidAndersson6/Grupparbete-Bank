using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupparbete_Bank
{
    public enum UserRole
    {
        Admin,
        Customer
    }
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int FailedLoginAttempts { get; private set; }
        public bool IsLocked { get; private set; }
        public UserRole Role { get; private set; }
        public DateTime LockedUntil { get; private set; } // Tid när kontot låses upp


        public List<BankAccount> Accounts { get; private set; }


        public User(string username, string password, UserRole role)
        {
            this.Password = password;
            this.Username = username;
            this.FailedLoginAttempts = 0;
            this.IsLocked = false;
            this.Role = role;
            this.Accounts = new List<BankAccount>();
            this.LockedUntil = DateTime.MinValue; // Ingen låsning 

        }
        public bool CheckPassword(string password)
        {
            return Password == password;
        }

        public void AddFailedAttempts()
        {
            FailedLoginAttempts++;

            if (FailedLoginAttempts >= 3)
            {
                //IsLocked = true;
                LockAccount();  // Lås kontot efter tre misslyckade inloggningar

                Console.WriteLine("Du har misslyckts för många gånger. Kontot låser sig nu..");
            }
        }
        public void ResetFailedAttempts()
        {
            FailedLoginAttempts = 0;
        }

        public void LockAccount()
        {
            IsLocked = true;
            LockedUntil = DateTime.Now.AddMinutes(1);  // Kontot låses i 1 minut
        }

        public void UnlockAccount()
        {
            IsLocked = false;
            LockedUntil = DateTime.MinValue; // Återställ låsningen
            Console.WriteLine("Kontot har återaktiverats.");
        }

        public void AddAccount(BankAccount account)

        {
            Accounts.Add(account);
        }

        public bool TransferBetweenAccount(string fromAccountNumber, string toAccountNumber, decimal amount)
        {
            BankAccount fromAccount = Accounts.Find(acc => acc.AccountNumber == fromAccountNumber);
            BankAccount toAccount = Accounts.Find(acc => acc.AccountNumber == toAccountNumber);

            if(fromAccount == null || toAccount == null)
            {
                Console.WriteLine("Ett av de angivna kontona hittades inte /:");
                return false;
            }

            if(fromAccount.Balance < amount)
            {
                Console.WriteLine("Du har tyvärr inte tillräckligt med pengar");
                return false;
            }

            fromAccount.Withdraw(amount);
            toAccount.Deposit(amount);

            Console.WriteLine($"Överföring av {amount:C2} från konto {fromAccountNumber} till konto {toAccountNumber} har skett!");
            return true;
        }


    }
}
