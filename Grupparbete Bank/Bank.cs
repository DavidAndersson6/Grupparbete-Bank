using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupparbete_Bank
{
    public class Bank
    {
        private List<User> users;

        public Bank()
        {
            users = new List<User>();

            users.Add(new User("admin", "admin123", UserRole.Admin));

        }
        public void RegisterUser(string username, string password, UserRole role)
        {
            if (users.Exists(u => u.Username == username))
            {
                Console.WriteLine("Användarnamnet du uppgav är upptaget");
                return;
            }

            users.Add(new User(password, username, role));
            Console.WriteLine($"Användare är skapad: {username}");
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

    }
}
