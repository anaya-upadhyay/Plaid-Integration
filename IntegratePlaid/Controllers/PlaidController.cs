using System.Net;

using IntegratePlaid.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using RestSharp;

namespace IntegratePlaid.Controllers
{
    public class PlaidController : Controller
    {
        private readonly ILogger<PlaidController> _logger;
        private readonly PlaidSettingsConfiguration _plaidSettings;

        public PlaidController(ILogger<PlaidController> logger,
                               IOptions<PlaidSettingsConfiguration> options)
        {
            _logger = logger;
            _plaidSettings = options.Value;
        }

        public IActionResult Index()
        {
            var client = new RestClient(_plaidSettings.LinkTokenURL);
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");

            var param = new PlaidParams
            {
                client_id = _plaidSettings.ClientId,
                secret = _plaidSettings.ClientSecret,
                client_name = "Wealthlane",
                country_codes = new List<string> { "US" },
                language = "en",
                user = new PlaidUser { client_user_id = "Anaya Upadhyay" },
                products = new List<string> { "auth" }
            };

            request.AddJsonBody(param);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<LinkResponse>(response.Content);
                ViewBag.LinkToken = result.link_token;
            }

            return View();
        }

        internal class PlaidParams
        {
            public string client_id { get; set; }
            public string secret { get; set; }
            public string client_name { get; set; }
            public List<string> country_codes { get; set; }
            public string language { get; set; }
            public PlaidUser user { get; set; }
            public List<string> products { get; set; }
        }

        internal class PlaidUser
        {
            public string client_user_id { get; set; }
        }

        /// <code>
        /// {
        ///   "expiration": "2022-08-30T23:35:28Z",
        ///   "link_token": "link-sandbox-5c97b355-a9da-4dec-98a1-3b0d0093c01d",
        ///   "request_id": "Bg6iD3kKeMfbzmP"
        /// }
        /// </code>
        internal class LinkResponse
        {
            public string expiration { get; set; }
            public string link_token { get; set; }
            public string request_id { get; set; }
        }
    }
}