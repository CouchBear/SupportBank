using System.Xml.Serialization;
[XmlRoot(ElementName = "SupportTransaction")]
class Transaction
{   [XmlAttribute("Date")]
    public string? Date { get; set; }
    [XmlElement("From")]
    public string? FromAccount { get; set; }
    [XmlElement("To")]
    public string?  ToAccount { get; set; }
    [XmlElement("Description")]
    public string? Narrative { get; set; }
    [XmlElement("Value")]
    public decimal? Amount { get; set; }

  public Transaction(){}
    public Transaction(string date,string fromAccount, string toAccount, string narrative, decimal amount){
        Date=date;
        FromAccount=fromAccount;
        ToAccount=toAccount;
        Narrative=narrative;
        Amount=amount;
    }
}