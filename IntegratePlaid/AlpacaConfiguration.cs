namespace IntegratePlaid
{
    public class AlpacaConfiguration
    {
        public string PaperAPIEndpoint { get; set; }
        public string PaperAPIKeyId { get; set; }
        public string PaperAPISecretKey { get; set; }

        public string BaseUrl { get; set; }
        public string AuthRedirection { get; set; }
        public string AuthToken { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }

        public string StockBaseURL { get; set; }
        /// <summary>
        /// The Quotes API provides NBBO quotes for a given ticker symbol over a specified time period.
        /// </summary>
        /// <remarks>
        /// Returns quotes (NBBOs) for the queried stock symbol.
        /// </remarks>
        public string Quotes { get; set; }
        /// <summary>
        /// The Multi Quotes API provides NBBO quotes for multiple given ticker symbols over a specified time period.
        /// </summary>
        /// <remarks>
        /// Returns quotes (NBBOs) for the queried stock symbols.
        /// </remarks>
        public string MultiQuotes { get; set; }
        /// <summary>
        /// The Latest Quote API provides the latest quote data for a given ticker symbol.
        /// </summary>
        /// <remarks>
        /// This endpoint returns the latest quote data for the requested security.
        /// </remarks>
        public string LatestQuote { get; set; }
        /// <summary>
        /// The Latest Multi Quotes API provides the latest NBBO quotes for multiple given stock symbols.
        /// </summary>
        /// <remarks>
        /// Returns quotes (NBBOs) for the queried stock symbols.
        /// </remarks>
        public string LatestMultiQuotes { get; set; }

        public string ClientID2 { get; set; }
        public string ClientSecret2 { get; set; }
    }
}
