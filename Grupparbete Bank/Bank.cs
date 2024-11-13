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
            users.Add(new User("admin", "admin123", UserRole.Admin));
            users.Add(new User("kund", "kund123", UserRole.Customer));

            exchangeRates = new Dictionary<string, decimal>();
            SetDefaultExchangeRates();
        }
        private void SetDefaultExchangeRates()
        {
            exchangeRates["SEK->USD"] = 0.10m;
            exchangeRates["USD->SEK"] = 10.0m;
            exchangeRates["SEK->EUR"] = 0.09m;
            exchangeRates["EUR->SEK"] = 11.0m;
        }
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
                Console.WriteLine("Välj valuta för kundens konto:");
                foreach (Currency currency in Enum.GetValues(typeof(Currency)))
                {
                    Console.WriteLine($"{(int)currency}: {currency}");
                }

                if (Enum.TryParse(Console.ReadLine(), out Currency selectedCurrency))
                {
                    string accountNumber = "100" + (users.Count + 1).ToString(); // Generera unikt kontonummer
                    newUser.AddAccount(new BankAccount(accountNumber, AccountType.Sparkonto, selectedCurrency, 0));
                    Console.WriteLine($"Kundkonto skapades med valuta: {selectedCurrency}");
                }

                else
                {
                    Console.WriteLine("Ogiltig valuta. Kontot skapas med standardvalutan SEK.");
                    newUser.AddAccount(new BankAccount("100" + (users.Count + 1).ToString(), AccountType.Sparkonto, Currency.SEK, 0));
                }

                //newUser.AddAccount(new BankAccount("100" + (users.Count + 1).ToString(), AccountType.Sparkonto, 0)); // Skapar ett konto med unikt kontonummer
            }

            users.Add(newUser);//Användare sparad i listan
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
                Console.WriteLine("Ditt konto är låst på grund av för många misslyckade inloggningsförsök.");
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
        public void CreateUser(User adminUser, string username, string password, UserRole role)
        {
            if (adminUser.Role != UserRole.Admin)
            {
                Console.WriteLine("Endast administratörer kan skapa nya användare.");
                return;
            }
            RegisterUser(username, password, role);

        }

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
            return amount; // Om ingen kurs finns, ingen växling sker
        }

        public bool TransferToOtherUser(User sender, string toAccountNumber, decimal amount)
        {
            // Kontrollera att avsändarens konto har täckning
            BankAccount fromAccount = sender.Accounts.FirstOrDefault(acc => acc.Balance >= amount);
            if (fromAccount == null)
            {
                Console.WriteLine("Du har inte tillräckligt med saldo.");
                return false;
            }

            // Leta efter mottagarkontot
            BankAccount toAccount = users
                .SelectMany(user => user.Accounts)
                .FirstOrDefault(acc => acc.AccountNumber == toAccountNumber);

            if (toAccount == null)
            {
                Console.WriteLine("Mottagarkontot hittades inte.");
                return false;
            }

            // Utför växling om kontona har olika valutor
            decimal amountToTransfer = amount;
            if (fromAccount.AccountCurrency != toAccount.AccountCurrency)
            {
                amountToTransfer = ConvertCurrency(amount, fromAccount.AccountCurrency, toAccount.AccountCurrency);
            }

            // Gör överföringen
            fromAccount.Withdraw(amount);
            toAccount.Deposit(amountToTransfer);

            Console.WriteLine($"Överföring av {amountToTransfer} {toAccount.AccountCurrency} från {sender.Username} till konto {toAccountNumber} lyckades!");
            return true;
        }

    }
}
