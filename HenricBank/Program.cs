using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BankApp app = new BankApp();
            app.Run();

        }
    }


    public class BankApp
    {
        private Dictionary<string, User> users = new Dictionary<string, User>(); // Ny dictionary 
        private User currentUser;
        private LoginManager loginManager = new LoginManager();

        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8; //Stöder specialtecken
            Console.WriteLine("Välkommen till FlammanBank :)");
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(@" 
   ⠀⠀⠀⢱⣆⠀⠀⠀⠀⠀⠀
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
");
            Console.ResetColor();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nVälj ett alternativ:");
                Console.WriteLine("1. Registrera");
                Console.WriteLine("2. Logga in");
                Console.WriteLine("3. Avsluta");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": //Skriver ut alla cases / alternativ
                        Register();
                        break;
                    case "2":
                        Login();
                        break;
                    case "3":
                        exit = true;
                        Console.WriteLine("Hejdå!");
                        break;
                    default:
                        Console.WriteLine("Nånting vart fel försök igen.");
                        break;
                }

                if (currentUser != null)
                {
                    Console.WriteLine($"\nVälkommen, {currentUser.Username}!");
                    ShowBankOptions();
                }
            }
        }

        private void Register()
        {
            Console.Write("Ange ett unikt användarnamn: ");
            string username = Console.ReadLine();

            if (users.ContainsKey(username))
            {
                Console.WriteLine("Användarnamnet finns redan. Försök med ett annat.");
                return;
            }

            Console.Write("Ange ett lösenord: ");
            string password = Console.ReadLine();

            User newUser = new User(username, password);
            users[username] = newUser;

            Console.WriteLine("Registrering lyckades! Du kan nu logga in.");
        }

        private void Login()
        {
            Console.Write("Ange ditt användarnamn: ");
            string username = Console.ReadLine();

            if (loginManager.IsLockedOut(username))
            {
                Console.WriteLine("Konto är tillfälligt låst. Försök igen senare.");
                return;
            }

            Console.Write("Ange ditt lösenord: ");
            string password = Console.ReadLine();

            if (users.TryGetValue(username, out User user) && user.Password == password)
            {
                currentUser = user;
                Console.WriteLine("Inloggning lyckades!");
                loginManager.ResetAttempts(username); // Återställ försök.???
            }
            else
            {
                Console.WriteLine("Ogiltigt användarnamn eller lösenord.");
                loginManager.RecordFailedAttempt(username);
            }
        }

        private void ShowBankOptions()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nVälj ett alternativ:");
                Console.WriteLine("1. Kontrollera saldo");
                Console.WriteLine("2. Insättning");
                Console.WriteLine("3. Uttag");
                Console.WriteLine("4. Logga ut");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        currentUser.CheckBalance();
                        break;
                    case "2":
                        currentUser.Deposit();
                        break;
                    case "3":
                        currentUser.Withdraw();
                        break;
                    case "4":
                        exit = true;
                        currentUser = null;
                        Console.WriteLine("Utloggning lyckades.");
                        break;
                    default:
                        Console.WriteLine("Ogiltigt alternativ. Försök igen.");
                        break;

                        //Console.WriteLine("Avändaren   ");
                }
            }
        }
    }



}

public class User
{
    public string Username { get; }
    public string Password { get; }
    private decimal balance;

    public User(string username, string password)
    {
        Username = username;
        Password = password;
        balance = 0;
    }

    public void CheckBalance()
    {
        Console.WriteLine($"Ditt saldo är: {balance:C}");
    }

    public void Deposit()
    {
        Console.Write("Ange belopp att sätta in: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
        {
            balance += amount;
            Console.WriteLine($"Insatta {amount:C}. Nytt saldo: {balance:C}");
        }
        else
        {
            Console.WriteLine("Ogiltigt belopp. Ange ett positivt nummer.");
        }
    }

    public void Withdraw()
    {
        Console.Write("Ange belopp att ta ut: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
        {
            if (amount <= balance)
            {
                balance -= amount;
                Console.WriteLine($"Uttagna {amount:C}. Nytt saldo: {balance:C}");
            }
            else
            {
                Console.WriteLine("Otillräckligt saldo.");
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt belopp. Ange ett positivt nummer.");
        }
    }
}
// case 2

//case 1
