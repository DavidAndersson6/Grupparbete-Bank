using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Grupparbete_Bank
{
    public class BankApp
    {
        private Bank bank;
        private User loggedInUser;

        public BankApp()
        {
            bank = new Bank();

        }
        public void Run()
        {
            Console.WriteLine("Välkommen till banken!\n");
            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("======Meny======");
                Console.WriteLine("Välj ett alternativ!");
                Console.WriteLine("1: Logga in");
                Console.WriteLine("2: Avsluta");
                Console.Write("Val: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        isRunning = false;
                        Console.WriteLine("Tack för att du använde banken. Du loggas nu ut..");
                        break;
                    default:
                        Console.WriteLine("Felaktigt alternativ. Försök igen!");
                        break;
                }
            }
        }
        public void Login()
        {
            Console.Write("Ange ditt Användarnamn: ");
            string username = Console.ReadLine();

            Console.Write("Ange ditt Lösenord: ");
            string password = Console.ReadLine();

            loggedInUser = bank.Login(username, password);

            if (loggedInUser != null)
            {
                Console.WriteLine($"Välkommen, {loggedInUser.Username}!");

                //loggedInUser.AddAccount(new BankAccount("2001", AccountType.Saldokonto,Currency.SEK ,20000));

                if (loggedInUser.Role == UserRole.Admin)
                {
                    ShowAdminMenu();
                }
                else { ShowUserMenu(); }
            }
            else
            {
                Console.WriteLine("Inloggningen misslyckades.");
            }
        }
        private void ShowAdminMenu()
        {
            bool inAdminMenu = true;

            while (inAdminMenu)
            {
                Console.WriteLine("\n======Admin-Meny======");
                Console.WriteLine("Välj ett alternativ!");
                Console.WriteLine("1: Skapa en ny kund");
                Console.WriteLine("2: Logga ut");
                Console.Write("Val: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateUser();
                        break;

                    case "2":
                        inAdminMenu = false;
                        Console.WriteLine("Utloggad från Admin menyn!");
                        loggedInUser = null;
                        break;

                    default:
                        Console.WriteLine("Felaktig input! försök igen...");
                        break;
                }
            }
        }
        private void CreateUser()
        {
            Console.Write("Ange ett nytt användarnamn: ");
            string username = Console.ReadLine();

            Console.Write("Ange ett lösenord för användaren: ");
            string password = Console.ReadLine();

            // Välj kontovaluta
            Console.WriteLine("Välj valuta för användarens konto:");
            foreach (Currency currency in Enum.GetValues(typeof(Currency)))
            {
                Console.WriteLine($"{(int)currency}: {currency}");
            }

            //bank.RegisterUser(username, password, UserRole.Customer);

            Currency selectedCurrency;
            if (Enum.TryParse(Console.ReadLine(), out selectedCurrency))
            {
                // Skapa användaren och konto med vald valuta
                User newUser = bank.RegisterUser(username, password, UserRole.Customer);

                if (newUser != null)
                {
                    Console.Write("Ange startbelopp för kontot: ");
                    decimal startBalance = decimal.Parse(Console.ReadLine());

                    string accountNumber = $"ACC{new Random().Next(1000, 9999)}"; // Generera ett konto-ID
                    BankAccount newAccount = new BankAccount(accountNumber, AccountType.Saldokonto, selectedCurrency, startBalance);

                    newUser.AddAccount(newAccount);
                    Console.WriteLine($"Användare och konto skapade med {selectedCurrency} som valuta.");
                }
            }
            else
            {
                Console.WriteLine("Ogiltig valuta. Försök igen.");
            }
        }

        public void ShowUserMenu()
        {
            bool inUserMenu = true;

            while (inUserMenu)
            {
                Console.WriteLine("\n======Användarmenu======");
                Console.WriteLine("Välj ett alternativ!");
                Console.WriteLine("1: Visa konton och saldo");
                Console.WriteLine("2: Överför pengar mellan egna konton");
                Console.WriteLine("3: Se Transaktionslogg");
                Console.WriteLine("4: Logga ut");
                Console.Write("Val: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowAccounts();
                        break;
                    case "2": TransferBetweenUserAccounts();
                        break;
                    case "3":
                        LogViewer.ShowLog();
                        break;
                    case "4":
                        TransferToOtherUser();
                        break;
                    case "4":
                        inUserMenu = false;
                        Console.WriteLine("Du loggas nu ut från user menu...");
                        loggedInUser = null;
                        break;
                    default: Console.WriteLine("Felaktig input! Försök igen...");
                        break;
                }
            }
        }
        private void ShowAccounts()
        {
            foreach(var account in loggedInUser.Accounts)
            {
                Console.WriteLine($"Kontonummer: {account.AccountNumber} | Typ: {account.Type} | Saldo: {account.Balance} | Valuta: {account.AccountCurrency}");

            }
        }
        private void TransferBetweenUserAccounts()
        {
            Console.WriteLine("Här kan du överföra pengar mellan dina konton");
            ShowAccounts();

            Console.Write("Mata kontnumret som du vill överföra pengar ifrån: ");
            string fromAccount = Console.ReadLine();

            Console.Write("Mata in kontonumret som du vill överföra pengar till: ");
            string toAccount = Console.ReadLine();

            Console.Write("Ange hur mycket du vill överföra: ");
            if(decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                bool succes = loggedInUser.TransferBetweenAccount(fromAccount, toAccount, amount);

                if (succes)
                {
                    Console.WriteLine($"Överföringen av {amount} från {fromAccount} lyckades");
                    Transaction.TransactionLogger.Instance.AddLogEntry(
               transactionType: "Internal Transfer",
               amount: amount,
               sourceAccount: fromAccount,
               destinationAccount: toAccount,
               description: "Internal transfer between user accounts"
           );
                    

                }
                else { Console.WriteLine("Överföringen misslyckades. Kontrollera saldot eller kontonumret"); }

            }
                
            else { Console.WriteLine("Ogiltigt beloppp. Försök igen..."); }
        }

        private void TransferToOtherUser()
        {
            Console.WriteLine("Överför pengar till ett annat konto");

            Console.WriteLine("Ange mottagarens kontonummer");
            string toAccountNumber = Console.ReadLine();

            Console.WriteLine("Ange beloppet som du vill överföra");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                bool success = bank.TransferToOtherUser(loggedInUser, toAccountNumber, amount);
                if (!success)
                {
                    Console.WriteLine("Överföringen misslyckades. Kontrollera beloppet eller kontonumret.");
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt belopp. Försök igen...");
            }
        }
    }
}
