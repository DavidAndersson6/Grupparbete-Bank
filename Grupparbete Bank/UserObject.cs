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


        public User(string username, string password, UserRole role)
        {
            this.Password = password;
            this.Username = username;
            this.FailedLoginAttempts = 0;
            this.IsLocked = false;
            this.Role = role;
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
                IsLocked = true;
                Console.WriteLine("Du har misslyckts för många gånger. Kontot låser sig nu..");
            }
        }
        public void ResetFailedAttempts()
        {
            FailedLoginAttempts = 0;
        }
    }
}
