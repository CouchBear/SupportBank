// See https://aka.ms/new-console-template for more information

using NLog;
using NLog.Config;
using NLog.Targets;
using System.IO;

var config = new LoggingConfiguration();
var target = new FileTarget { FileName = @"C:\Work\Logs\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
config.AddTarget("File Logger", target);
config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
LogManager.Configuration = config;

//create an empty list of strings
List<Account> accounts = new List<Account>();
List<Transaction> transactions = new List<Transaction>();
string fileName = "./Transactions2014.csv";
//string fileName = "./DodgyTransactions2015.csv";
//string fileName="./Transactions2013.json";
//string fileName="./Transactions2012.xml";

string toFileName="./mydata.csv";
Budget newBudget=new Budget(accounts, transactions, fileName);
newBudget.ListAll();
newBudget.ListAccount();
newBudget.ListAllToFile(toFileName);