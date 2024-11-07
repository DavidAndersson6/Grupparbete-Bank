namespace InitialBacklog;

public class Bank
{
    public SavingsAccount OpenSavingsAccount(BankCustomer customer, double depositeAmount)
    {
        SavingsAccount newSavingsAccount = new SavingsAccount();
        customer.Balance += depositeAmount;

        double interest = newSavingsAccount.CalculateDepositInterest(depositeAmount);
        Console.WriteLine($"Om du sätter in {depositeAmount} kronor får du {interest} kronor i ränta.");

        return newSavingsAccount;
    }

    public void GiveLoan(BankCustomer customer, double balance)
    {
        if (CanLoan(customer, balance))
        {
            double loanInterest = 0.05;
            double totalInterest = balance * loanInterest;
            customer.Loan += balance;
            Console.WriteLine($"Du lånar {balance} kronor och kommer behöva betala {totalInterest} kronor i ränta.");
        }
        else
        {
            Console.WriteLine("Du kan inte låna detta belopp eftersom det överstiger din lånegräns.");
        }
    }

    public bool CanLoan(BankCustomer customer, double loanAmount)
    {
        double maximumLoan = customer.Balance * 5;
        return loanAmount <= maximumLoan;
    }
}