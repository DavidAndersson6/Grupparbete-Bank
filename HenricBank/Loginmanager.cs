using System;
using System.Collections.Generic;

public class LoginManager
{
    private const int MaxAttempts = 3;
    private const int LockoutDuration = 60; // Lockout duration in seconds

    private readonly Dictionary<string, (int attempts, DateTime? lockoutEndTime)> loginAttempts =
        new Dictionary<string, (int attempts, DateTime? lockoutEndTime)>();

    public void RecordFailedAttempt(string username)
    {
        if (!loginAttempts.ContainsKey(username))
        {
            loginAttempts[username] = (1, null);
        }
        else
        {
            var (attempts, lockoutEndTime) = loginAttempts[username];
            attempts++;

            if (attempts >= MaxAttempts)
            {
                Console.WriteLine("För många misslyckade försök. Kontot är låst i en minut.");
                loginAttempts[username] = (attempts, DateTime.Now.AddSeconds(LockoutDuration));
            }
            else
            {
                loginAttempts[username] = (attempts, lockoutEndTime);
            }
        }
    }

    public bool IsLockedOut(string username)
    {
        if (loginAttempts.TryGetValue(username, out var loginData))
        {
            var (attempts, lockoutEndTime) = loginData;

            if (lockoutEndTime.HasValue && lockoutEndTime > DateTime.Now)
            {
                TimeSpan remaining = lockoutEndTime.Value - DateTime.Now;
                Console.WriteLine($"Konto låst. Försök igen om {remaining.Seconds} sekunder.");
                return true;
            }

            if (lockoutEndTime.HasValue && lockoutEndTime <= DateTime.Now)
            {
                ResetAttempts(username);  // Reset after lockout expires
            }
        }
        return false;
    }

    public void ResetAttempts(string username)
    {
        loginAttempts[username] = (0, null);
    }
}