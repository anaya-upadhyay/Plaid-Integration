using System.Data.Common;
using System.Net;

using Alpaca.Markets;

using IntegratePlaid.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

using Environments = Alpaca.Markets.Environments;

namespace IntegratePlaid.Controllers
{
    public class AlpacaController : Controller
    {
        private readonly AlpacaConfiguration _config;

        private IAlpacaDataClient _alpacaDataClient;

        private readonly Interval<DateTime> _timeInterval = new(DateTime.Today.AddDays(-1), DateTime.Today);
        private readonly string[] _stocks = { "AAPL", "MSFT" };


        public AlpacaController(IOptions<AlpacaConfiguration> options)
        {
            _config = options.Value;

            // First, open the API connection
            _alpacaDataClient = Environments.Live
                .GetAlpacaDataClient(new SecretKey(_config.ClientID, _config.ClientSecret));
        }

        public IActionResult Index()
        {
            #region Alpha Vantage

            //string QUERY_URL = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=IBM&apikey=536NLXHNY7D07WHK";
            //Uri queryUri = new Uri(QUERY_URL);

            //using (WebClient client = new WebClient())
            //{
            //    // -------------------------------------------------------------------------
            //    // if using .NET Framework (System.Web.Script.Serialization)

            //    //dynamic json_data = JsonConvert.DeserializeObject(client.DownloadString(queryUri), typeof(object));

            //    // -------------------------------------------------------------------------
            //    // if using .NET Core (System.Text.Json)
            //    // using .NET Core libraries to parse JSON is more complicated. For an informative blog post
            //    // https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-apis/

            //    var result = client.DownloadString(queryUri);

            //    //dynamic json_data = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri));

            //    // -------------------------------------------------------------------------

            //    // do something with the json_data
            //} 

            #endregion

            return View(new List<Quotes>());
        }

        [HttpPost]
        public IActionResult Index(string symbol)
        {
            //if(string.IsNullOrEmpty(symbol)) symbol = "";

            symbol ??= "AAPL,TSLA";
            List<Quotes> quotes = new();

            try
            {
                var client = new RestClient($"{_config.StockBaseURL}{_config.LatestMultiQuotes}?symbols={symbol}");

                var request = new RestRequest();
                request.Method = Method.Get;
                request.AddHeader("APCA-API-KEY-ID", _config.ClientID2);
                request.AddHeader("APCA-API-SECRET-KEY", _config.ClientSecret2);

                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    //ViewBag.Quotes = quotes;
                    quotes = DeserializeQuotes(response.Content);
                    //Console.WriteLine(response.Content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception => {ex}");
            }

            return View(quotes);

        }

        private List<Quotes> DeserializeQuotes(string response)
        {
            var quoteResult = new List<Quotes>();
            
            JObject quotes = JObject.Parse(response);

            var anaya = from p in quotes["quotes"]
                        select p;

            foreach (var item in anaya)
            {
                var itemQuote = new Quotes();

                var mySymbol = item as JProperty;

                itemQuote.Name = mySymbol.Name;

                Console.WriteLine($"Symbol: {mySymbol.Name}");

                if(item.Children().Children().Count() > 0)
                {
                    var quoteResponseResult = new QuoteResponse();
                    object qr = null;

                    foreach (var child in item.Children().Children())
                    {
                        mySymbol = child as JProperty;
                        var tokenValue = mySymbol.Value;                        

                        qr = mySymbol.Name switch
                        {
                            "t" => quoteResponseResult.Timestamp = Convert.ToDateTime(tokenValue),
                            "ax" => quoteResponseResult.AskExchange = tokenValue.ToString(),
                            "ap" => quoteResponseResult.AskPrice = Convert.ToDecimal(tokenValue),
                            "as" => quoteResponseResult.AskSize=Convert.ToInt32(tokenValue),
                            "bx" => quoteResponseResult.BidExchange = tokenValue.ToString(),
                            "bp" => quoteResponseResult.BidPrice = Convert.ToDecimal(tokenValue),
                            "bs" => quoteResponseResult.BidSize=Convert.ToInt32(tokenValue),
                            "c" => quoteResponseResult.QuoteConditions = new List<string> { tokenValue.ToString()},
                            "z" => quoteResponseResult.Tape = tokenValue.ToString(),
                            _ => ""
                        };
                    }

                    itemQuote.Data = quoteResponseResult;
                }
                quoteResult.Add(itemQuote);
            }

            return quoteResult;
        }

        #region Commented Codes

        //public async Task OauthRedirection()
        //{
        //    try
        //    {
        //        var client = new RestClient($"{_config.BaseUrl}{_config.AuthRedirection}" +
        //            $"?response_type=code" +
        //            $"&client_id={_config.ClientID}" +
        //            $"&redirect_uri=https://localhost:7284/" +
        //            $"&state=ANAYA_UPADHYAY" +
        //            $"&scope=data");

        //        var request = new RestRequest();
        //        request.Method = Method.Get;

        //        var response = client.Execute(request);

        //        if (response.IsSuccessful)
        //        {
        //            await Task.CompletedTask;
        //        }

        //        Console.WriteLine(response.Content);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Exception => {ex}");
        //    }
        //}

        //public IActionResult AlpacaAuth()
        //{
        //    return View();
        //}

        //private async Task GetHistoricalData()
        //{
        //    try
        //    {
        //        // Get daily price data for AAPL over the last 5 trading days
        //        var quotes = await _alpacaDataClient.ListHistoricalQuotesAsync(new HistoricalQuotesRequest(_stocks, _timeInterval));

        //        if (quotes.Items.Any())
        //        {
        //            foreach (var item in quotes.Items)
        //            {
        //                Console.WriteLine($"Ask Price = {item.AskPrice}; "
        //                                  + $"\nBid Price = {item.BidPrice}"
        //                                  + $"\nTime = {item.TimestampUtc}"
        //                                  + $"\nAsk Size = {item.AskSize}"
        //                                  + $"\nBid Size = {item.BidSize}");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }
        //}

        //private async Task GetRealtimeData()
        //{
        //    var client = Environments.Paper.GetAlpacaCryptoStreamingClient(new SecretKey(_config.PaperAPIKeyId, _config.PaperAPISecretKey));

        //    await client.ConnectAndAuthenticateAsync();

        //    string symbol = "BTCUSD";

        //    var barSubscription = client.GetMinuteBarSubscription(symbol);
        //    barSubscription.Received += (bar) =>
        //    {
        //        Console.WriteLine(bar);
        //    };

        //    await client.SubscribeAsync(barSubscription);
        //    while (true) ;
        //}

        #endregion
    }
}
