using System.Data;
using Newtonsoft.Json;
using NLog;



class FileReader
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    public static List<Transaction> CsvFileReader(string fileName, List<Transaction> transactions)
    {
        using (var reader = new StreamReader(fileName))
        {  
            string newLine;
            bool isFirstLine = true;
            while ((newLine = reader.ReadLine()) != null)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }
                string[] newLineArray = newLine.Split(",");
                Logger.Info("create a new transaction using array information and add it to the transactions list");
                transactions.Add(new Transaction(newLineArray[0], newLineArray[1], newLineArray[2], newLineArray[3], Convert.ToDecimal(newLineArray[4])));
            }
        }
        return transactions;
    }

    public static List<Transaction> JsonFileReader(string fileName, List<Transaction> transactions)
    {
        Logger.Info("reading json file to create a list of Transactions");
        using (StreamReader r = new StreamReader(fileName))
        {
            string json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Transaction>>(json);
        }
    }

    public static List<Transaction> XmlFileReader(string fileName, List<Transaction> transactions)
    {   Logger.Info("reading xml file to create a list of Transactions");
        var xmlString = File.ReadAllText(fileName);
        var stringReader = new StringReader(xmlString);
        var dsSet = new DataSet();
        dsSet.ReadXml(stringReader);

        // Define the join query
        var joinQuery =
            from row1 in dsSet.Tables["SupportTransaction"].AsEnumerable()
            join row2 in dsSet.Tables["Parties"].AsEnumerable()
            on row1.Field<int>("SupportTransaction_Id") equals row2.Field<int>("SupportTransaction_Id")
            select new
            {   // Define the columns to include in the result
                Date = row1.Field<string>("Date"),
                FromAccount = row2.Field<string>("From"),
                ToAccount = row2.Field<string>("To"),
                Narrative = row1.Field<string>("Description"),
                Amount = row1.Field<string>("Value")
            };

        
        foreach (var item in joinQuery)
        {transactions.Add(new Transaction(item.Date,item.FromAccount,item.ToAccount,item.Narrative,Convert.ToDecimal(item.Amount)));
        }
        return transactions;
    }
    }
