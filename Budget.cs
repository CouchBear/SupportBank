using System.Text.Json;
using NLog;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;


class Budget
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    //create an empty list of strings
    public List<Account> Accounts { get; set; }
    public List<Transaction> Transactions { get; set; }
    public string FileName { get; set; }

    public Budget(List<Account> accounts, List<Transaction> transactions, string fileName)
    {

        Accounts = accounts;
        Transactions = transactions;
        FileName = fileName;
        Logger.Info("Read csv file into reader");
        string extension = Path.GetExtension(FileName);
        try
        {
            switch (extension)
            {
                case ".csv":
                    Transactions = FileReader.CsvFileReader(FileName, Transactions);
                    break;

                case ".json":
                    Transactions = FileReader.JsonFileReader(FileName, Transactions);
                    break;

                case ".xml":
                    Transactions = FileReader.XmlFileReader(FileName, Transactions);
                    break;
                default:
                    {
                        Console.WriteLine("unknown file type");
                        break;
                    }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurs in reading file and building Transaction objects" + ex.Message);
        }
        try
        {

            foreach (Transaction t in Transactions)
            {
                if (!Accounts.Any(x => x.Name == t.FromAccount))
                { Accounts.Add(new Account(t.FromAccount)); }
                if (!Accounts.Any(x => x.Name == t.ToAccount))
                { Accounts.Add(new Account(t.ToAccount)); }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error with building Account object" + ex.Message);
        }

    }

    public void ListAll()
    {
        try
        {
            foreach (var account in Accounts)
            {
                decimal totalAmount = 0;
                foreach (var trans in Transactions)
                {
                    if (account.Name == trans.FromAccount)
                    {
                        totalAmount += Convert.ToDecimal(trans.Amount);
                    }
                    if (account.Name == trans.ToAccount)
                    { totalAmount -= Convert.ToDecimal(trans.Amount); }
                }
                Console.WriteLine($"Account Name: {account.Name}, Total Amount: {totalAmount}");
            }
        }
        catch (Exception ex)
        { Console.Write("An Error Occured while trying to list all accounts" + ex.Message); }
    }


    public void ListAccount()
    {
        Console.WriteLine("Please enter the name you want to see the account");
        string userInputName = Console.ReadLine();
        Console.WriteLine($"Transactions for {userInputName}");
        try
        {
            foreach (var trans in Transactions)
            {
                if (userInputName == trans.FromAccount || userInputName == trans.ToAccount)
                {
                    Console.WriteLine($"Date: {trans.Date}, From: {trans.FromAccount}, To: {trans.ToAccount}, Narrative: {trans.Narrative}, Amount: {trans.Amount}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured while trying to list account for {userInputName}" + ex.Message);
        }

    }

    public void ListAllToFile(string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            try
            {
                foreach (var account in Accounts)
                {
                    decimal totalAmount = 0;
                    foreach (var trans in Transactions)
                    {
                        if (account.Name == trans.FromAccount)
                        {
                            totalAmount += Convert.ToDecimal(trans.Amount);
                        }
                        if (account.Name == trans.ToAccount)
                        { totalAmount -= Convert.ToDecimal(trans.Amount); }
                    }
                    writer.WriteLine($"Account Name: {account.Name}, Total Amount: {totalAmount}");
                }
            }
            catch (Exception ex)
            { writer.WriteLine("An Error Occured while trying to list all accounts" + ex.Message); }
        }

    }
}





