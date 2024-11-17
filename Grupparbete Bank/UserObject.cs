using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupparbete_Bank
{
    public enum UserRole
    {
        Admin, // Defines whether the user is an admin
        Customer // Defines whether the user is a customer
    }
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int FailedLoginAttempts { get; private set; }
        public bool IsLocked { get; private set; }
        public UserRole Role { get; private set; }
        public DateTime LockedUntil { get; private set; } // Tid när kontot låses upp
        public decimal TotalLoanAmount { get; set; } = 0m; // Totalt lånebelopp för användaren



        public List<BankAccount> Accounts { get; private set; }

        // Constructor: Initializes a User object with a username, password, role, and an empty account list
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
        // Verifies if the provided password matches the user's password
        public bool CheckPassword(string password)
        {
            return Password == password;
        }

        // Increases the count of failed login attempts and locks the account if the threshold is reached
        public void AddFailedAttempts()
        {
            FailedLoginAttempts++;

            if (FailedLoginAttempts >= 3)
            {
                LockAccount(); 

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
            LockedUntil = DateTime.Now.AddMinutes(1);  //Account locked for 1 minute
        }

        // Unlocks the account manually
        public void UnlockAccount()
        {
            IsLocked = false;
            LockedUntil = DateTime.MinValue; 
            Console.WriteLine("Kontot har återaktiverats.");
        }

        public void AddAccount(BankAccount account)

        {
            Accounts.Add(account);
        }

        // Transfers money between the user's own accounts if sufficient funds are available
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

            Console.WriteLine($"Överföring av {amount} från konto {fromAccountNumber} till konto {toAccountNumber} har skett!");
            return true;
        }


    }
}
