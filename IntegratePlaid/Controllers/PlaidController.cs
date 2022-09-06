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

        private const string PLAID_SANDBOX = "sandbox";
        private const string PLAID_DEVELEOPMENT = "development";

        public PlaidController(ILogger<PlaidController> logger,
                               IOptions<PlaidSettingsConfiguration> options)
        {
            _logger = logger;
            _plaidSettings = options.Value;
        }

        public IActionResult Index()
        {
            var client = new RestClient(_plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.LinkTokenURL : _plaidSettings.DevTokenURL);
                        
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");

            var param = new PlaidParams
            {
                client_id = _plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.ClientId: _plaidSettings.DevClientId,
                secret = _plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.ClientSecret: _plaidSettings.DevSecret,
                client_name = "Wealthlane",
                country_codes = new List<string> { "US" },
                language = "en",
                user = new PlaidUser { client_user_id = "Anaya Upadhyay" },
                products = new List<string> { "auth" },

                // Un-omment the following line if you want to use Micro-Deposit
                //auth = new LinkAuth { same_day_microdeposits_enabled = true, auth_type_select_enabled = true }
            };

            request.AddJsonBody(param);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<LinkResponse>(response.Content);
                ViewBag.LinkToken = result.link_token;
            }

            ViewBag.Environment = _plaidSettings.Environment;

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
            /// <summary>
            /// Add the following property for Micro-Deposit.
            /// This is supported only on ["auth"] product.
            /// </summary>
            public LinkAuth auth { get; set; }
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

        /// <summary>
        /// For Micro-Deposit
        /// </summary>
        internal class LinkAuth
        {
            public bool same_day_microdeposits_enabled { get; set; }
            public bool auth_type_select_enabled { get; set; }
        }
    }
}