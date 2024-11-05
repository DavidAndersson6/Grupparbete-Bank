using System;
using System.Collections.Generic;
using System.Linq;
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

                if (loggedInUser.Role == UserRole.Admin)
                {
                    ShowAdminMenu();
                }
                else { Console.WriteLine("Inloggad som kund"); }
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
                Console.WriteLine("1: Skapa en ny användare");
                Console.WriteLine("2: Avsluta");
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

            Console.Write("Ange roll för andvändaren du skapar. (Admin/Customer): ");
            string roleInput = Console.ReadLine();

            UserRole role = roleInput.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? UserRole.Admin : UserRole.Customer;
            bank.CreateUser(loggedInUser, username, password, role);
        }
    }
}
