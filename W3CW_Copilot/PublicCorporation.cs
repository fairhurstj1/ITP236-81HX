public class PublicCorporation
{
    private static readonly PublicCorporation[] virginiaCorporations =
    [
        new("NOC", "Northrop Grumman", "Falls Church, VA", 75_000_000_000m),
        new("GD", "General Dynamics", "Reston, VA", 70_000_000_000m),
        new("RTX", "RTX Corporation", "Arlington, VA", 150_000_000_000m),
        new("HLT", "Hilton Worldwide Holdings", "McLean, VA", 50_000_000_000m),
        new("COF", "Capital One Financial", "McLean, VA", 55_000_000_000m),
        new("DLTR", "Dollar Tree", "Chesapeake, VA", 28_000_000_000m),
        new("KMX", "CarMax", "Richmond, VA", 11_000_000_000m),
        new("BAH", "Booz Allen Hamilton", "McLean, VA", 19_000_000_000m),
        new("CACI", "CACI International", "Reston, VA", 10_000_000_000m),
        new("AES", "The AES Corporation", "Arlington, VA", 12_000_000_000m)
    ];

    public static List<PublicCorporation> VirginiaCorporations => [.. virginiaCorporations];

    public static Dictionary<string, PublicCorporation> VirginiaCorporationsBySymbol =>
        virginiaCorporations.ToDictionary(c => c.StockSymbol, StringComparer.OrdinalIgnoreCase);

    public static HashSet<string> VirginiaStockSymbols =>
        virginiaCorporations.Select(c => c.StockSymbol).ToHashSet(StringComparer.OrdinalIgnoreCase);

    public static Queue<PublicCorporation> VirginiaCorporationQueue =>
        new(virginiaCorporations);

    public static Stack<PublicCorporation> VirginiaCorporationStack =>
        new(virginiaCorporations);

    public string StockSymbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal MarketCap { get; set; }

    public PublicCorporation() { }
    public PublicCorporation(
        string stockSymbol,
        string name,
        string location,
        decimal marketCap)
    {
        StockSymbol = string.IsNullOrWhiteSpace(stockSymbol)
            ? throw new ArgumentException("Stock symbol is required.", nameof(stockSymbol))
            : stockSymbol.Trim().ToUpperInvariant();

        Name = string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentException("Name is required.", nameof(name))
            : name.Trim();

        Location = string.IsNullOrWhiteSpace(location)
            ? throw new ArgumentException("Location is required.", nameof(location))
            : location.Trim();

        MarketCap = marketCap < 0
            ? throw new ArgumentOutOfRangeException(nameof(marketCap), "Market cap cannot be negative.")
            : marketCap;
    }

    public string GetFormattedMarketCap()
    {
        const decimal million = 1_000_000m;
        const decimal billion = 1_000_000_000m;
        const decimal trillion = 1_000_000_000_000m;

        if (MarketCap >= trillion)
        {
            return $"${MarketCap / trillion:0.##}T";
        }

        if (MarketCap >= billion)
        {
            return $"${MarketCap / billion:0.##}B";
        }

        if (MarketCap >= million)
        {
            return $"${MarketCap / million:0.##}M";
        }

        return $"{MarketCap:C}";
    }

    public override string ToString()
    {
        return $"{Name} ({StockSymbol}) | {Location} | Market Cap: {GetFormattedMarketCap()}";
    }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(StockSymbol)
            && !string.IsNullOrWhiteSpace(Name)
            && !string.IsNullOrWhiteSpace(Location)
            && MarketCap >= 0;
    }

}

