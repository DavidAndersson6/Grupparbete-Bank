namespace InitialBacklog;

public class SavingsAccount
{
    public double InterestRate { get; set; } = 0.04;

    public double CalculateDepositInterest(double deposit)
    {
        return deposit * InterestRate;
    }
}