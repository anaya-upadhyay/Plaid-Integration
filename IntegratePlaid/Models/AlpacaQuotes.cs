using NuGet.Packaging.Signing;

namespace IntegratePlaid.Models
{
    public class AlpacaQuotes
    {
        public List<Quotes> Quotes { get; set; }
    }

    public class Quotes
    {
        public string Name { get; set; }
        public QuoteResponse Data { get; set; }
    }

    public class QuoteResponse
    {
        public DateTime Timestamp { get; set; }
        public string AskExchange { get; set; }
        public decimal AskPrice { get; set; }
        public int AskSize { get; set; }
        public string BidExchange { get; set; }
        public decimal BidPrice { get; set; }
        public int BidSize { get; set; }
        public List<string> QuoteConditions { get; set; }
        public string Tape { get; set; }

    }
}
