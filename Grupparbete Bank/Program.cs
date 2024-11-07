namespace InitialBacklog;

public class Program
{
    static void Main(string[] args)
    {
        BankService bankService = new BankService();
        bankService.StartBankServices();
    }
}