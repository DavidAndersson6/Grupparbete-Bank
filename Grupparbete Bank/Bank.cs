using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Grupparbete_Bank
{
    public class Bank
    {
        private List<User> users;
        private Dictionary<string, decimal> exchangeRates;


        public Bank()
        {
            users = new List<User>();
            users.Add(new User("admin", "admin123", UserRole.Admin)); // Default admin user
            users.Add(new User("kund", "kund123", UserRole.Customer)); // Default customer user

            exchangeRates = new Dictionary<string, decimal>();
            SetDefaultExchangeRates();
        }

        // Sets the default exchange rates between supported currencies
        private void SetDefaultExchangeRates()
        {
            exchangeRates["SEK->USD"] = 0.10m;
            exchangeRates["USD->SEK"] = 10.0m;
            exchangeRates["SEK->EUR"] = 0.09m;
            exchangeRates["EUR->SEK"] = 11.0m;
        }

        // Allows users to register with the bank, including the creation of an account
        public User RegisterUser(string username, string password, UserRole role)
        {
            if (users.Exists(u => u.Username == username))
            {
                Console.WriteLine("Användarnamnet du uppgav är upptaget");
                return null;
            }

            User newUser = new User(username, password, role);



            if(role == UserRole.Customer)
            {
                // Allow the customer to select a currency for their savings account
                Console.WriteLine("Välj valuta för sparkontot:");
                foreach (Currency currency in Enum.GetValues(typeof(Currency)))
                {
                    Console.WriteLine($"{(int)currency}: {currency}");
                }

                if (Enum.TryParse(Console.ReadLine(), out Currency selectedCurrency))
                {
                    string accountNumber = "100" + (users.Count + 1).ToString(); 
                    newUser.AddAccount(new BankAccount(accountNumber, AccountType.Sparkonto, selectedCurrency, 0));
                   // Console.WriteLine($"Ett sparkonto har skapats med valuta: {selectedCurrency}");
                }

                else
                {
                    Console.WriteLine("Ogiltig valuta. Kontot skapas med standardvalutan SEK.");
                    newUser.AddAccount(new BankAccount("100" + (users.Count + 1).ToString(), AccountType.Sparkonto, Currency.SEK, 0));
                }

                //newUser.AddAccount(new BankAccount("100" + (users.Count + 1).ToString(), AccountType.Sparkonto, 0)); // Skapar ett konto med unikt kontonummer
            }

            users.Add(newUser);
            Console.WriteLine($"Användare {username} är skapad");
            return newUser;
        }

        public User Login(string username, string password)
        {
            User user = users.Find(u => u.Username == username);

            if (user == null)
            {
                Console.WriteLine("Användaren hittades inte");
                return null;
            }

            if (user.IsLocked)
            {
                Console.WriteLine("Ditt konto är låst på grund av för många misslyckade inloggningsförsök (1min).");
                return null;
            }

            if (user.CheckPassword(password))
            {
                Console.WriteLine("\nInloggningen lyckades!");
                user.ResetFailedAttempts();
                return user;
            }

            else
            {
                Console.WriteLine("Felaktigt lösenord.");
                user.AddFailedAttempts();
                return null;
            }
        }

        // Allows admins to create new users (customers or other admins)
        public void CreateUser(User adminUser, string username, string password, UserRole role)
        {
            if (adminUser.Role != UserRole.Admin)
            {
                Console.WriteLine("Endast administratörer kan skapa nya användare.");
                return;
            }
            RegisterUser(username, password, role);

        }

        // Updates the exchange rate between two currencies
        public void UpdateExchangeRate(string fromCurrency, string toCurrency, decimal rate)
        {
            string key = $"{fromCurrency}->{toCurrency}";
            exchangeRates[key] = rate;
            Console.WriteLine($"Växelkursen för {fromCurrency} till {toCurrency} uppdaterades till {rate}");
        }

        private decimal ConvertCurrency(decimal amount, Currency fromCurrency, Currency toCurrency)
        {
            string key = $"{fromCurrency}->{toCurrency}";
            if (exchangeRates.TryGetValue(key, out decimal rate))
            {
                return amount * rate;
            }
            Console.WriteLine("Ingen växelkurs tillgänglig för denna valutakombination.");
            return amount; // No conversion if the rate is unavailable
        }

        public bool TransferToOtherUser(User sender, string toAccountNumber, decimal amount)
        {
            // Checking that the user account has enough balance
            BankAccount fromAccount = sender.Accounts.FirstOrDefault(acc => acc.Balance >= amount);
            if (fromAccount == null)
            {
                Console.WriteLine("Du har inte tillräckligt med saldo.");
                return false;
            }

            // Looking for the recivers account
            BankAccount toAccount = users
                .SelectMany(user => user.Accounts)
                .FirstOrDefault(acc => acc.AccountNumber == toAccountNumber);

            if (toAccount == null)
            {
                Console.WriteLine("Mottagarkontot hittades inte.");
                return false;
            }

            // Doing the exchange if the accounts has different currencies
            decimal amountToTransfer = amount;
            if (fromAccount.AccountCurrency != toAccount.AccountCurrency)
            {
                amountToTransfer = ConvertCurrency(amount, fromAccount.AccountCurrency, toAccount.AccountCurrency);
            }

            // Complete the transfer
            fromAccount.Withdraw(amount);
            toAccount.Deposit(amountToTransfer);

            Console.WriteLine($"Överföring av {amountToTransfer} {toAccount.AccountCurrency} från {sender.Username} till konto {toAccountNumber} lyckades!");
            return true;
        }

    }
}
