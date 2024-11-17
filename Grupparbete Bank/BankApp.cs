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
        private string bankTitleArt = @"
  ______ _                                       ____              _    
 |  ____| |                                     |  _ \            | |   
 | |__  | | __ _ _ __ ___  _ __ ___   __ _ _ __ | |_) | __ _ _ __ | | __
 |  __| | |/ _` | '_ ` _ \| '_ ` _ \ / _` | '_ \|  _ < / _` | '_ \| |/ /
 | |    | | (_| | | | | | | | | | | | (_| | | | | |_) | (_| | | | |   < 
 |_|    |_|\__,_|_| |_| |_|_| |_| |_|\__,_|_| |_|____/ \__,_|_| |_|_|\_\
";
        private string titleArt = @" 
   ⠀   ⢱⣆⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠈⣿⣷⡀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢸⣿⣿⣷⣧⠀⠀⠀
⠀⠀⠀⠀⡀⢠⣿⡟⣿⣿⣿⡇⠀⠀
⠀⠀⠀⠀⣳⣼⣿⡏⢸⣿⣿⣿⢀⠀
⠀⠀⠀⣰⣿⣿⡿⠁⢸⣿⣿⡟⣼⡆
⢰⢀⣾⣿⣿⠟⠀⠀⣾⢿⣿⣿⣿⣿
⢸⣿⣿⣿⡏⠀⠀⠀⠃⠸⣿⣿⣿⡿
⢳⣿⣿⣿⠀⠀⠀⠀⠀⠀⢹⣿⡿⡁
⠀⠹⣿⣿⡄⠀⠀⠀⠀⠀⢠⣿⡞⠁
⠀⠀⠈⠛⢿⣄⠀⠀⠀⣠⠞⠋⠀⠀
⠀⠀⠀⠀⠀⠀⠉⠀
";
        public BankApp()
        {
            bank = new Bank();

        }
        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8; // Supports special characters in console output
            Console.ForegroundColor = ConsoleColor.Red; // Sets color for the bank title display
            Console.WriteLine(bankTitleArt);
            Console.WriteLine(titleArt);
            Console.ResetColor();
            bool isRunning = true;

            // Main menu loop
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

                // Admin menu actions
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
            // Handles the creation of a new customer user
            Console.Write("Ange ett nytt användarnamn: ");
            string username = Console.ReadLine();

            Console.Write("Ange ett lösenord för användaren: ");
            string password = Console.ReadLine();

            // Välj kontovaluta
            Console.WriteLine("Välj valuta för saldokontot:");
            foreach (Currency currency in Enum.GetValues(typeof(Currency)))
            {
                Console.WriteLine($"{(int)currency}: {currency}");
            }

            //bank.RegisterUser(username, password, UserRole.Customer);

            Currency selectedCurrency;
            if (Enum.TryParse(Console.ReadLine(), out selectedCurrency))
            {
                // Register the user and create an account with the selected currency
                User newUser = bank.RegisterUser(username, password, UserRole.Customer);

                if (newUser != null)
                {
                    Console.Write("Ange startbelopp för saldokontot: ");
                    decimal startBalance = decimal.Parse(Console.ReadLine());

                    // Generate a unique account number and create the account
                    string accountNumber = $"ACC{new Random().Next(1000, 9999)}";
                    BankAccount newAccount = new BankAccount(accountNumber, AccountType.Saldokonto, selectedCurrency, startBalance);

                    newUser.AddAccount(newAccount);
                    Console.WriteLine($"Ett saldokonto och ett sparkonto har skapats");
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

            // Main menu for loggen-in users
            // The menu loops until the user logs out.

            while (inUserMenu)
            {
                Console.WriteLine("\n======Användarmenu======");
                Console.WriteLine("Välj ett alternativ!");
                Console.WriteLine("1: Visa konton och saldo");
                Console.WriteLine("2: Överför pengar mellan egna konton");
                Console.WriteLine("3: Överför pengar till ett annat konto");
                Console.WriteLine("4: Ta lån");
                Console.WriteLine("5: Se Transaktionslogg (Intern överföring)");
                Console.WriteLine("6: Logga ut");
                Console.Write("Val: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowAccounts();
                        break;
                    case "2": TransferBetweenUserAccounts();
                        break;
                    case "3":
                        TransferToOtherUser();
                        break;
                    case "4":
                        LoanMoney();
                        break;
                    case "5":
                        LogViewer.ShowLog();
                        break;
                    case "6":
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
            // Displays all accounts belonging to the logged-in user.
            foreach (var account in loggedInUser.Accounts)
            {
                Console.WriteLine($"Kontonummer: {account.AccountNumber} | Typ: {account.Type} | Saldo: {account.Balance} | Valuta: {account.AccountCurrency}");

            }
            Console.WriteLine();
        }
        private void TransferBetweenUserAccounts()
        {
            // Allows the user to transfer funds between their own accounts.

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
                    // Logs the transfer if successful

                    var sourceAccount = loggedInUser.Accounts.FirstOrDefault(a => a.AccountNumber == fromAccount);
                    if (sourceAccount != null)
                    {
                        Currency selectedCurrency = sourceAccount.AccountCurrency;
                        Transaction.TransactionLogger.Instance.AddLogEntry(
               transactionType: "Internal Transfer",
               amount: amount,
               sourceAccount: fromAccount,
               destinationAccount: toAccount,
               description: "Internal transfer between user accounts",
               sourceAccountCurrency: selectedCurrency.ToString()

           );
                    }

                }
                else { Console.WriteLine("Överföringen misslyckades. Kontrollera saldot eller kontonumret"); }

            }
                
            else { Console.WriteLine("Ogiltigt beloppp. Försök igen..."); }
        }

        private void TransferToOtherUser()
        {
            // Enables the user to transfer money to another user's account.

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

        public void LoanMoney()
        {
            // Allows the user to request a loan with a maximum limit based on their account balance.
            if (loggedInUser == null || loggedInUser.Role != UserRole.Customer)
            {
                Console.WriteLine("Endast inloggade kunder kan ansöka om lån.");
                return;
            }

            Console.Write("Ange beloppet du vill låna: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal loanAmount) && loanAmount > 0)
            {
                decimal totalBalance = loggedInUser.Accounts.Sum(account => account.Balance);
                decimal maxLoanAmount = totalBalance * 5;  // Maximum loan is 5x total balance

                if (loggedInUser.TotalLoanAmount + loanAmount > maxLoanAmount)
                {
                    Console.WriteLine($"Du kan inte låna mer än 5 gånger ditt saldo i detta fall ({maxLoanAmount}).");
                    return;
                }

                decimal interestRate = 0.05m; // Fixed interest rate (5%)
                decimal interestAmount = interestRate * loanAmount;

                Console.WriteLine($"Om du lånar {loanAmount} kommer du behöva betala {interestAmount} i ränta.");

                Console.Write("Vill du gå vidare? Ja/Nej: ");
                string conformation = Console.ReadLine().Trim().ToLower();

                if(conformation != "ja") 
                {
                    Console.WriteLine("Lånet avbröts");
                    return;
                }

                // Approve loan and deposit the amount into the user's primary account
                BankAccount primaryAccount = loggedInUser.Accounts.FirstOrDefault();
                if (primaryAccount != null)
                {
                    primaryAccount.Deposit(loanAmount);
                    loggedInUser.TotalLoanAmount += loanAmount; // Uppdatera användarens totala lånebelopp
                    Console.WriteLine($"Lån godkänt! {loanAmount} har lagts till på kontonumret {primaryAccount.AccountNumber}.");
                }
                else
                {
                    Console.WriteLine("Inget giltigt konto hittades att lägga lånet på.");
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt belopp. Försök igen.");
            }
        }
    }
}
