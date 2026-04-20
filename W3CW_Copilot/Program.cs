var corporationList = PublicCorporation.VirginiaCorporations;
var coStar = new PublicCorporation("CSGP", "CoStar Group, Inc.", "Virginia", 30_000_000_000m);
var altria = new PublicCorporation("MO", "Altria Group, Inc.", "Virginia", 80_000_000_000m);
var carMax = new PublicCorporation("KMX", "CarMax, Inc.", "Virginia", 20_000_000_000m);
corporationList.Add(coStar);
corporationList.Add(altria);
corporationList.Add(carMax);

var corp = corporationList.FirstOrDefault(c => c.StockSymbol == "CSGP");
if (corp != null)
{
    corporationList.Remove(corp);
}
corp = corporationList.FirstOrDefault(c => c.StockSymbol == "ALTR");
if (corp != null)
{
    corporationList.Remove(corp);
}


Console.WriteLine();
// Console.WriteLine("Interest Rate Calculator");

// var calculator = new InterestRateCalculator();

// decimal amount;
// while (true)
// {
// 	Console.Write("Enter amount: ");
// 	if (decimal.TryParse(Console.ReadLine(), out amount) && amount > 0)
// 	{
// 		break;
// 	}

// 	Console.WriteLine("Please enter a valid amount greater than 0.");
// }

// int termMonths;
// while (true)
// {
// 	Console.Write("Enter term in months: ");
// 	if (int.TryParse(Console.ReadLine(), out termMonths) && termMonths > 0)
// 	{
// 		break;
// 	}

// 	Console.WriteLine("Please enter a valid number of months greater than 0.");
// }

// var annualRate = calculator.GetAnnualInterestRate(amount, termMonths);
// var interest = calculator.CalculateInterest(amount, termMonths);
// var totalAmount = calculator.CalculateTotalAmount(amount, termMonths);

// Console.WriteLine();
// Console.WriteLine($"Annual Interest Rate: {annualRate:P2}");
// Console.WriteLine($"Interest Amount: {interest:C}");
// Console.WriteLine($"Total Amount: {totalAmount:C}");

//Dictionary manipulation
var corporationDict = PublicCorporation.VirginiaCorporationsBySymbol;
corporationDict.Values.ToList();
corporationDict.Add("CSGP", coStar);
corp = corporationDict.GetValueOrDefault("CSGP");
if (corp != null)
{
    corporationDict.Remove("CSGP");
}

//HashSet manipulation
var stockSymbols = PublicCorporation.VirginiaStockSymbols;
corp = corporationList.FirstOrDefault(c => c.StockSymbol == "ADP");
if (corp != null)
{
    stockSymbols.Remove(corp.StockSymbol);
}
//print hashset
Console.WriteLine("Virginia Stock Symbols:");
foreach (var symbol in stockSymbols)
{
    Console.WriteLine(symbol);
}
stockSymbols.Add("CSGP");

//Queue manipulation

var corporationQueue = PublicCorporation.VirginiaCorporationQueue;
corporationQueue.Enqueue(coStar);
corp = corporationQueue.FirstOrDefault(c => c.StockSymbol == "ADP");
if (corp != null)
{
    var tempQueue = new Queue<PublicCorporation>();
    while (corporationQueue.Count > 0)
    {
        var current = corporationQueue.Dequeue();
        if (current.StockSymbol != "ADP")
        {
            tempQueue.Enqueue(current);
        }
    }
    while (tempQueue.Count > 0)
    {
        corporationQueue.Enqueue(tempQueue.Dequeue());
    }
}
//Stack manipulation
var corporationStack = PublicCorporation.VirginiaCorporationStack;
corporationStack.Push(coStar);
corp = corporationStack.FirstOrDefault(c => c.StockSymbol == "ADP");
if (corp != null)
{
    var tempStack = new Stack<PublicCorporation>();
    while (corporationStack.Count > 0)
    {
        var current = corporationStack.Pop();
        if (current.StockSymbol != "ADP")
        {
            tempStack.Push(current);
        }
    }
    while (tempStack.Count > 0)
    {
        corporationStack.Push(tempStack.Pop());
    }
}
