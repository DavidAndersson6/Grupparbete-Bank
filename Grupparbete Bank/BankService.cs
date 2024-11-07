using InitialBacklog;

public class BankService
{
    private Bank bank;
    private BankCustomer customer;

    public BankService()
    {
        bank = new Bank();
        customer = new BankCustomer(""); //Skriv in namn på kunden (för och efternamn)
    }

    public void StartBankServices()
    {
        Console.WriteLine("Hej! Välj nedanför ett av alternativen.2"); //Skrivs in på login funktionen
        Console.WriteLine("1. Öppna sparkonto och sätt in pengar: ");
        Console.WriteLine("2. Låna pengar från banken: ");

        Console.Write("Välj ett alternativ: ");
        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                OpenSavingsAccount();
                break;
            case 2:
                LoanMoney();
                break;
            default:
                Console.WriteLine("Ogiltigt val. Försök igen.");
                break;
        }
    }

    private void OpenSavingsAccount()
    {
        Console.WriteLine("Ange belopp att sätta in i sparkontot: ");
        double depositAmount = double.Parse(Console.ReadLine());
        bank.OpenSavingsAccount(customer, depositAmount);
    }

    private void LoanMoney()
    {
        Console.WriteLine("Ange lånebelopp: ");
        double loanAmount = double.Parse(Console.ReadLine());
        bank.GiveLoan(customer, loanAmount);
    }
}