namespace InitialBacklog;

public class BankCustomer
{
    public string Name { get; set; }
    public double Balance { get; set; }
    public double Loan { get; set; }

    public BankCustomer(string name)
    {
        Name = name;
        Balance = 0;
        Loan = 0;
    }
}